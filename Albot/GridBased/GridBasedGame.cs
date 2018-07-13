using System.Collections.Generic;
using System;

using static Albot.GridBased.GridBasedJsonHandler;

namespace Albot.GridBased {
    public abstract class GridBasedGame : AlbotConnection {

        protected int width, height;

        private string state;
        private bool stateUpToDate = false;

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

        public new void RestartGame() {
            base.RestartGame();
            stateUpToDate = false;
        }

        // Blocking receive 
        public GridBoard GetNextBoard() {
            string state = GetState();
            if (GameOver())
                return null;
            return ParseResponseState(state, width, height);
        }

        /// <summary>
        /// Makes the move given.
        /// </summary>
        public void MakeMove(int move) {
            stateUpToDate = false;

            /*JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.makeMove),
                new JProperty(Fields.move, move)
                );
            */
            SendCommand(move.ToString());
        }
        
        /// <summary>
        /// Returns a list of legal moves according to the board given.
        /// </summary>
        public List<int> GetPossibleMoves(GridBoard board) {
            
            string jCommand = CreateCommandPossibleMoves(board);
            string moves = SendCommandReceiveState(jCommand);
            //Console.WriteLine("Response possmoves: \n" + moves + "\n");
            return ParseResponsePossibleMoves(moves);
        }

        /// <summary>
        /// Returns a board in which the given move has been played by the given player.
        /// </summary>
        /// <param name="board">Board which the move should be applied to</param>
        /// <param name="player">Player to make the move</param>
        /// <param name="move">Move to be played</param>
        /// <returns></returns>
        public GridBoard SimulateMove(GridBoard board, int player, int move) {
            
            string jCommand = CreateCommandSimulateMove(board, player, move);

            string state = SendCommandReceiveState(jCommand);
            return ParseResponseState(state, width, height);
        }

        /// <summary>
        /// Returns the state of the board. (PlayerWon|EnemyWon|Draw|Ongoing)
        /// </summary>
        public BoardState EvaluateBoard(GridBoard board) {
            
            string jCommand = CreateCommandEvaluateBoard(board);

            string winner = SendCommandReceiveState(jCommand);
            return ParseResponseEvaluateBoard(winner);
        }
        
    }


    
}
