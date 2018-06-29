using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

using static Albot.Constants;

namespace Albot {
    public abstract class GridBasedGame : AlbotConnection {

        protected int width, height;

        private string state;
        private bool stateUpToDate = false;

        public GridBasedGame(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
            InitGridDimensions();
        }

        protected abstract void InitGridDimensions();
        
        // Multiple accesses for state would, without this, get into an infinite blocking receive.
        private string GetState() {
            if (stateUpToDate)
                return state;

            state = ReceiveState();
            stateUpToDate = true;
            return state;
        }

        public new bool GameOver() {
            GetState();
            return base.GameOver();
        }

        // Blocking receive
        public GridBoard GetNextBoard() {
            
            return new GridBoard(width, height, ParseResponseState(GetState()));
        }

        public void MakeMove(int move) {
            stateUpToDate = false;

            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.makeMove),
                new JProperty(Fields.move, move)
                );
            SendCommand(jsonCommand.ToString());
        }
        
        public int EvaluateBoard(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.evalBoard),
                new JProperty(Fields.board, board.Serialize())
                );

            string winner = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponseWinner(winner);
        }

        public List<int> GetPossibleMoves(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.getPossMoves),
                new JProperty(Fields.board, board.Serialize())
                );

            string moves = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponsePossibleMoves(moves);
        }
        
        public GridBoard SimulateMove(GridBoard board, int player, int move) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.simMove),
                new JProperty(Fields.board, board.Serialize()),
                new JProperty(Fields.player, player.ToString()),
                new JProperty(Fields.move, move)
                );

            string state = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponseState(state);
        }

        private GridBoard ParseResponseState(string response) {
            JObject jResponse = JObject.Parse(response);
            return new GridBoard(width, height, jResponse.GetValue(Fields.board).ToString());
        }

        // Optimize?
        private List<int> ParseResponsePossibleMoves(string response) {
            JObject jResponse = JObject.Parse(response);
            string moves = jResponse.GetValue(Fields.possibleMoves).ToString();
            return JsonConvert.DeserializeObject<List<int>>(moves);
        }

        private int ParseResponseWinner(string response) {
            JObject jResponse = JObject.Parse(response);
            string winner = jResponse.GetValue(Fields.winner).ToString();
            return int.Parse(winner);
        }
    }


    
}
