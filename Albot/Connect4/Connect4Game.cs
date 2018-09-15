using System;
using Albot.GridBased;

namespace Albot.Connect4 {

    /// <summary>
    /// A high level Connect4 library which sets up the connection and provides basic logic.
    /// </summary>
    public class Connect4Game : GridBasedGame {

        public Connect4Board currentBoard;
        
        public Connect4Game(string ip = "127.0.0.1", int port = 4000) : base(ip,port) {
            
        }

        protected override void InitGridDimensions() {
            this.width = Connect4Constants.Fields.boardWidth;
            this.height = Connect4Constants.Fields.boardHeight;
        }
        
        protected override void UpdateCurrentBoard(GridBoard board) {
            currentBoard = new Connect4Board(board);
        }
        
        /// <summary>
        /// Returns a board in which the given move has been applied by the given player.
        /// </summary>
        public Connect4Board SimulateMove(Connect4Board board, int player, int move) {
            GridBoard gridBoard = base.SimulateMove(board, player, move);
            return new Connect4Board(gridBoard);
        }

        /// <summary>
        /// Plays an entire game by making moves returned by the function provided. 
        /// </summary>
        public void PlayGame(Func<Connect4Board, int> decideMove, bool autoRestart) {

            while (true) {
                if (AwaitNextGameState() != BoardState.ongoing) {
                    if (autoRestart) {
                        RestartGame();
                        continue;
                    } else
                        break;
                }
                int move = decideMove(currentBoard);
                MakeMove(move);
            }

        }

    }
}
