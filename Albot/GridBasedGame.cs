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

        private const string BOARD = "Board";

        public GridBasedGame(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
            InitGridDimensions();
            Console.WriteLine("Connected, waiting for game to start...");
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
            string state = GetState();
            if (GameOver())
                return null;
            return new GridBoard(width, height, ParseResponseState(state));
        }

        /// <summary>
        /// Makes the move given.
        /// </summary>
        public void MakeMove(int move) {
            stateUpToDate = false;

            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.makeMove),
                new JProperty(Fields.move, move)
                );
            SendCommand(jsonCommand.ToString());
        }

        /// <summary>
        /// Returns the state of the board: 1 if you win, -1 if you lose, else 0.
        /// </summary>
        public int EvaluateBoard(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.evalBoard),
                new JProperty(BOARD, board.Serialize())
                );

            string winner = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponseWinner(winner);
        }

        /// <summary>
        /// Returns a list of legal moves according to the board given.
        /// </summary>
        public List<int> GetPossibleMoves(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.getPossMoves),
                new JProperty(BOARD, board.Serialize())
                );

            string moves = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponsePossibleMoves(moves);
        }
        
        public GridBoard SimulateMove(GridBoard board, int player, int move) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.simMove),
                new JProperty(BOARD, board.Serialize()),
                new JProperty(Fields.player, player.ToString()),
                new JProperty(Fields.move, move)
                );

            string state = SendCommandReceiveState(jsonCommand.ToString());
            return ParseResponseState(state);
        }

        private GridBoard ParseResponseState(string response) {
            JObject jResponse = JObject.Parse(response);
            //Console.WriteLine(jResponse.ToString());
            string serializedGrid = jResponse.GetValue(BOARD).ToString();
            //Console.WriteLine("Serialized grid: " + serializedGrid);
            return new GridBoard(width, height, serializedGrid);
        }

        // Optimize?
        private List<int> ParseResponsePossibleMoves(string response) {
            //Console.WriteLine(response);
            JObject jResponse = JObject.Parse(response);
            //Console.WriteLine(jResponse.ToString());
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
