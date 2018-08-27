using System;
using System.Text;
using System.Net.Sockets;

namespace Albot {
    
    /// <summary>
    /// Handles connection and transporting data between client and bot.
    /// </summary>
    public class AlbotConnection {

        private static TcpClient client;
        private static NetworkStream stream;
        private bool gameOver = false;
        private int winner;
        private const int bufferSize = 2048;

        /// <summary>
        /// Creates and handles the TCP connection with the client.
        /// </summary>
        public AlbotConnection(string ip = "127.0.0.1", int port = 4000) {

            try {
                client = new TcpClient(ip, port) {
                    ReceiveBufferSize = bufferSize
                    //SendBufferSize = 10000
                };
                stream = client.GetStream();
            } catch(Exception e) {
                Console.WriteLine("Could not establish TCP connection to Albot. \n" + e);
                Terminate();
            }
        }
        
        /// <summary>
        /// Blocking receive call for new TCP message as raw string.
        /// </summary>
        public string ReceiveMessage() {
            Byte[] data = new byte[bufferSize];

            //Console.WriteLine("Receiving data...");

            try {
                Int32 bytes = stream.Read(data, 0, data.Length);

                string incomingData;
                do {
                    incomingData = Encoding.Default.GetString(data, 0, bytes);

                } while (incomingData == null);// Blocking receive

                //Console.WriteLine("Data received: \n" + incomingData);

                HandleGameOverCheck(incomingData);

                return incomingData;
            } catch(Exception e) {
                Console.WriteLine("Could not receive message from TCP connection. \n" + e);
                Terminate();
            }
            return ""; // Unreachable
        }

        // Make move, should not be blocking? (It should be async)
        //public void SendCommand(int move) {SendCommand(move.ToString());}
        /// <summary>
        /// Sends the string to the client as a raw string.
        /// </summary>
        public void SendCommand(string command) {
            //Console.WriteLine("Sending command: \n" + command);
            try {
                byte[] response = Encoding.Default.GetBytes(command);
                stream.Write(response, 0, response.Length);
            } catch(Exception e) {
                Console.WriteLine("Could not send message with TCP connection. \n" + e);
                Terminate();
            }
            //Console.WriteLine("Command sent!");
        }

        //public string SendCommandRecvState(int command) { return SendCommandRecvState(command.ToString()); }
        /// <summary>
        /// Sends command and then does a blocking receive for a response.
        /// </summary>
        public string SendCommandReceiveMessage(string command) {
            SendCommand(command);
            return ReceiveMessage();
        }

        /// <summary>
        /// Sends a command to restart the game.
        /// </summary>
        public void RestartGame() {
            Console.WriteLine("Restarting game...");
            SendCommand(Constants.Actions.restartGame);
            gameOver = false;
        }

        /// <summary>
        /// Returns true if game is over, make sure to check this after receiving the state.
        /// </summary>
        public bool GameOver() {
            return gameOver;
        }
        /// <summary>
        /// Returns winner, 1 if you won, -1 if you lost, 0 if draw. Call this after GameOver() is true.
        /// </summary>
        public int GetWinner() {
            return winner;
        }

        private void HandleGameOverCheck(string incomingData) {
            incomingData = incomingData.Trim();
            if (incomingData.Length >= 8 && incomingData.Substring(0, 8) == Constants.Fields.gameOver) {
                if (incomingData.EndsWith("-1")) {
                    winner = -1;
                    Console.WriteLine("You lost!");
                } else if (incomingData.EndsWith("1")) {
                    winner = 1;
                    Console.WriteLine("You won!");
                } else if (incomingData.EndsWith("0")) {
                    winner = 0;
                    Console.WriteLine("Draw!");
                } else
                    Console.WriteLine("Game Over!");

                gameOver = true;
            }
        }

        // Hopefully makes sure the process is killed properly when program crashes. 
        static internal void Terminate() {
            if (client != null) {
                client.GetStream().Close();
                client.Close(); // Supposed to handle stream as well
            }
            Environment.Exit(1);
        }

    }
}
