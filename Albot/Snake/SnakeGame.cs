using System;
using System.Collections.Generic;
using System.Text;

using static Albot.Snake.SnakeStructs;

namespace Albot.Snake {

    /// <summary>
    /// A high level Snake library which sets up the connection and provides basic logic.
    /// </summary>
    public class SnakeGame : AlbotConnection {

        SnakeBoard currentBoard;

        /// <summary>
        /// Initializes library and connects to the client.
        /// </summary>
        public SnakeGame(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
            Console.WriteLine("Connected, waiting for game to start...");
        }

        /// <summary>
        /// Blocking receive call for next board. 
        /// </summary>
        public SnakeBoard GetNextBoard() {

            string state = ReceiveState(); // Receive before check for game over

            if (GameOver())
                return null;
            BoardStruct response = SnakeJsonHandler.ParseResponseState(state);
            currentBoard = new SnakeBoard(currentBoard, response);
            return currentBoard;
        }

        /// <summary>
        /// Make your move, sets the direction of your snake.
        /// </summary>
        public void MakeMove(string direction) {
            //SendCommand(SnakeJsonHandler.CreateDirectionCommand(direction));
            SendCommand(direction);
        }

        /// <summary>
        /// Returns the possible moves for both the player and the enemy based off only the directions. 
        /// </summary>
        public PossibleMoves GetPossibleMoves(SnakeBoard board) {
            string request = SnakeJsonHandler.CreateCommandPossibleMoves(board);
            string response = SendCommandReceiveState(request);
            return SnakeJsonHandler.ParseResponsePossibleMoves(response);
        }
        /*
        public SnakeBoard SimulatePlayerMove(SnakeBoard board, string move) {
            MovesToSimulate simMoves = new MovesToSimulate() { PlayerMove = move };
            return HandleSimulateMove(board, simMoves);
        }

        public SnakeBoard SimulateEnemyMove(SnakeBoard board, string move) {
            MovesToSimulate simMoves = new MovesToSimulate() {EnemyMove = move};
            return HandleSimulateMove(board, simMoves);
        }
        */

        /// <summary>
        /// Returns a board where the moves have been applied.
        /// </summary>
        public SnakeBoard SimulateMoves(SnakeBoard board, string playerMove, string enemyMove) {
            MovesToSimulate simMoves = new MovesToSimulate() { playerMove = playerMove, enemyMove = enemyMove };
            return HandleSimulateMove(board, simMoves);
        }

        private SnakeBoard HandleSimulateMove(SnakeBoard board, MovesToSimulate simMoves) {
            string request = SnakeJsonHandler.CreateCommandSimulate(board, simMoves);
            string response = SendCommandReceiveState(request);
            return SnakeJsonHandler.ParseResponseSimulate(response);
        }

        /// <summary>
        /// Returns the state of the board. 
        /// </summary>
        public BoardState EvaluateBoard(SnakeBoard board) {
            string request = SnakeJsonHandler.CreateCommandEvaluate(board);
            string response = SendCommandReceiveState(request);

            return SnakeJsonHandler.ParseResponseEvaluate(response);
        }

        /// <summary>
        /// Plays an entire game by making moves returned by the function provided. 
        /// </summary>
        public void PlayGame(Func<SnakeBoard, string> decideMove, bool autoRestart) {

            while (true) {
                SnakeBoard newBoard = GetNextBoard();
                if (GameOver()) {
                    if (autoRestart) {
                        RestartGame();
                        continue;
                    } else
                        break;
                }
                string move = decideMove(newBoard);
                MakeMove(move);
            }
            
        }



    }

}
