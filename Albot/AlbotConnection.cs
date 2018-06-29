using System;
using System.Text;
using System.Net.Sockets;

namespace Albot {

    public enum BoardState { PlayerWon, EnemyWon, Draw, Ongoing }

    /// <summary>
    /// Handles connection and transporting data between client and bot.
    /// </summary>
    public class AlbotConnection {

        private NetworkStream stream;
        private bool gameOver = false;
        private const int bufferSize = 1024;

        public AlbotConnection(string ip = "127.0.0.1", int port = 4000) {

            TcpClient client = new TcpClient(ip, port) {
                ReceiveBufferSize = bufferSize
                //SendBufferSize = 10000
            };
            stream = client.GetStream();
        }
        
        /// <summary>
        /// Blocking receive call for new state as raw string.
        /// </summary>
        public string ReceiveState() {
            Byte[] data = new byte[bufferSize];

            Int32 bytes = stream.Read(data, 0, data.Length);

            string incomingData;
            do {
                incomingData = Encoding.Default.GetString(data, 0, bytes);

            } while (incomingData == null);// Blocking receive

            HandleGameOverCheck(incomingData);

            //Console.WriteLine("Data received: \n" + incomingData + "\n\n");

            return incomingData;
        }

        // Make move, should not be blocking? (It should be async)
        //public void SendCommand(int move) {SendCommand(move.ToString());}
        /// <summary>
        /// Sends the string to the client as a raw string.
        /// </summary>
        public void SendCommand(string jsonCommand) {
            byte[] response = Encoding.Default.GetBytes(jsonCommand);
            stream.Write(response, 0, response.Length);
        }

        //public string SendCommandRecvState(int command) { return SendCommandRecvState(command.ToString()); }
        /// <summary>
        /// Sends command and then does a blocking receive for a response.
        /// </summary>
        public string SendCommandReceiveState(string jsonCommand) {
            SendCommand(jsonCommand);
            return ReceiveState();
        }

        /// <summary>
        /// Sends a command to restart the game.
        /// </summary>
        public void RestartGame() {
            Console.WriteLine("Restarting game...");
            SendCommand(Constants.Actions.restartGame);
        }

        /// <summary>
        /// Returns true if game is over, make sure to check this after receiving the state.
        /// </summary>
        public bool GameOver() {
            return gameOver;
        }

        private void HandleGameOverCheck(string incomingData) {
            incomingData = incomingData.Trim();
            if (incomingData.Length >= 8 && incomingData.Substring(0, 8) == Constants.Fields.gameOver) {
                if (incomingData.EndsWith("-1"))
                    Console.WriteLine("You lost!");
                else if (incomingData.EndsWith("1"))
                    Console.WriteLine("You won!");
                else
                    Console.WriteLine("Draw!");

                gameOver = true;
            }
        }

    }
}
