using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albot {

    /// <summary>
    /// Handles connection to the Albot client and provides general game functionality.
    /// </summary>
    public abstract class Game : AlbotConnection {

        /// <summary>
        /// Whether game is over and if so, who the winner is.
        /// </summary>
        public BoardState boardState;

        internal Game(string ip = "127.0.0.1", int port = 4000) : base(ip, port) {
            Console.WriteLine("Waiting for game to start...");
        }
        
        internal JObject ReceiveNextGameState() {
            string response = ReceiveMessage();
            JObject jObject = JsonHandler.TryParse(response);
            boardState = JsonHandler.ExtractBoardState(ref jObject);
            if(boardState != BoardState.ongoing)
                PrintGameOverMessage(boardState);
            return jObject;
        }

        /// <summary>
        /// Blocking receive call for next board and its state, both are stored locally as public variables. 
        /// </summary>
        /// <returns>The state of the board/game, check for ongoing if you want to see if game is over or not.</returns>
        public BoardState AwaitNextGameState() {
            JObject jState = ReceiveNextGameState();

            ExtractState(jState);

            return boardState;
        }
        
        protected abstract void ExtractState(JObject jState);

        /// <summary>
        /// Sends a command to restart the game.
        /// </summary>
        internal void RestartGame() {
            Console.WriteLine("Restarting game...");
            SendCommand(Constants.Actions.restartGame);
        }

        private void PrintGameOverMessage(BoardState boardState) {
            if (boardState == BoardState.playerWon)
                Console.WriteLine("You won!");
            else if (boardState == BoardState.enemyWon)
                Console.WriteLine("You lost!");
            else
                Console.WriteLine("Draw!");

        }


    }
}
