using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

using static Albot.Snake.SnakeStructs;

namespace Albot.Snake {

    /// <summary>
    /// A high level Snake library which sets up the connection and provides basic logic.
    /// </summary>
    public class SnakeGame : Game {
        
        public SnakeBoard currentBoard;

        /// <summary>
        /// Initializes library and connects to the client.
        /// </summary>
        public SnakeGame(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
        }

        protected override void ExtractState(JObject jState) {
            BoardStruct boardStruct = SnakeJsonHandler.ParseResponseState(jState);
            currentBoard = new SnakeBoard(currentBoard, boardStruct);
        }

        /// <summary>
        /// Make your move, sets the direction of your snake.
        /// </summary>
        public void MakeMove(string direction) {
            //SendCommand(SnakeJsonHandler.CreateDirectionCommand(direction));
            SendCommand(direction);
        }

        /// <summary>
        /// Returns the possible moves for both the player and the enemy, based off directions only. 
        /// </summary>
        public PossibleMoves GetPossibleMoves(SnakeBoard board) {
            string request = SnakeJsonHandler.CreateCommandPossibleMoves(board);
            string response = SendCommandReceiveMessage(request);
            return SnakeJsonHandler.ParseResponsePossibleMoves(response);
        }

        /// <summary>
        /// Simulate a move where only the player moves.
        /// </summary>
        /// <param name="board">The board which the move is applied to.</param>
        /// <param name="move">The direction in which you want to simulate a move.</param>
        /// <returns>The board where the move has been applied.</returns>
        public SnakeBoard SimulatePlayerMove(SnakeBoard board, string move) {
            MovesToSimulate simMoves = new MovesToSimulate() { playerMove = move };
            string request = SnakeJsonHandler.CreateCommandSimulatePlayer(board, simMoves);
            return HandleSimulateMove(board, simMoves, request);
        }

        /// <summary>
        /// Simulate a move where only the enemy moves.
        /// </summary>
        /// <param name="board">The board which the move is applied to.</param>
        /// <param name="move">The direction in which you want to simulate a move.</param>
        /// <returns>The board where the move has been applied.</returns>
        public SnakeBoard SimulateEnemyMove(SnakeBoard board, string move) {
            MovesToSimulate simMoves = new MovesToSimulate() {enemyMove = move};
            string request = SnakeJsonHandler.CreateCommandSimulateEnemy(board, simMoves);
            return HandleSimulateMove(board, simMoves, request);
        }

        /// <summary>
        /// Returns a board where the moves have been applied.
        /// </summary>
        public SnakeBoard SimulateMoves(SnakeBoard board, string playerMove, string enemyMove) {
            MovesToSimulate simMoves = new MovesToSimulate() { playerMove = playerMove, enemyMove = enemyMove };
            string request = SnakeJsonHandler.CreateCommandSimulateBoth(board, simMoves);
            return HandleSimulateMove(board, simMoves, request);
        }

        private SnakeBoard HandleSimulateMove(SnakeBoard board, MovesToSimulate simMoves, string request) {
            string response = SendCommandReceiveMessage(request);
            return SnakeJsonHandler.ParseResponseSimulate(response);
        }

        /// <summary>
        /// Returns the state of the board. 
        /// </summary>
        public BoardState EvaluateBoard(SnakeBoard board) {
            string request = SnakeJsonHandler.CreateCommandEvaluate(board);
            string response = SendCommandReceiveMessage(request);

            return SnakeJsonHandler.ParseResponseEvaluate(response);
        }

        /// <summary>
        /// Sends a command to restart the game.
        /// </summary>
        public new void RestartGame() {
            currentBoard = null;
            base.RestartGame();
        }

        /// <summary>
        /// Plays an entire game by making moves returned by the function provided. 
        /// </summary>
        public void PlayGame(Func<SnakeBoard, string> decideMove, bool autoRestart) {

            while (true) {
                if (AwaitNextGameState() != BoardState.ongoing) { // GameOver
                    if (autoRestart) {
                        RestartGame();
                        continue;
                    } else
                        break;
                }

                string move = decideMove(currentBoard);
                MakeMove(move);
            }
            
        }
        
    }

}
