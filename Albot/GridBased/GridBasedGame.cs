using System.Collections.Generic;
using System;

using static Albot.GridBased.GridBasedJsonHandler;
using Newtonsoft.Json.Linq;

namespace Albot.GridBased {
    public abstract class GridBasedGame : Game {

        protected int width, height;

        private GridBoard currentBoard;

        public GridBasedGame(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
            InitGridDimensions();
        }

        protected abstract void InitGridDimensions();

        protected override void ExtractState(JObject jState) {
            currentBoard = ParseResponseState(jState, width, height);
            UpdateCurrentBoard(currentBoard);
        }

        protected abstract void UpdateCurrentBoard(GridBoard board);


        /// <summary>
        /// Sends a command to restart the game.
        /// </summary>
        public new void RestartGame() {
            currentBoard = null;
            base.RestartGame();
        }

        /// <summary>
        /// Makes the move given.
        /// </summary>
        public void MakeMove(int move) {
            SendCommand(move.ToString());
        }
        
        /// <summary>
        /// Returns a list of legal moves according to the board given.
        /// </summary>
        public List<int> GetPossibleMoves(GridBoard board) {
            
            string jCommand = CreateCommandPossibleMoves(board);
            string moves = SendCommandReceiveMessage(jCommand);
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

            string state = SendCommandReceiveMessage(jCommand);
            return ParseResponseSimulateMove(state, width, height);
        }

        /// <summary>
        /// Returns the state of the board. (PlayerWon|EnemyWon|Draw|Ongoing)
        /// </summary>
        public BoardState EvaluateBoard(GridBoard board) {
            
            string jCommand = CreateCommandEvaluateBoard(board);

            string winner = SendCommandReceiveMessage(jCommand);
            return ParseResponseEvaluateBoard(winner);
        }
        
    }


    
}
