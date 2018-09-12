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
        private const int bufferSize = 2048;

        /// <summary>
        /// Creates and handles the TCP connection with the client.
        /// </summary>
        public AlbotConnection(string ip = "127.0.0.1", int port = 4000) {

            try {
                client = new TcpClient(ip, port) {
                    ReceiveBufferSize = bufferSize,
                    SendBufferSize = bufferSize
                };
                stream = client.GetStream();
                Console.WriteLine("Connected to Albot!");
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

        // Hopefully makes sure the process is killed properly when program crashes. 
        static internal void Terminate() {
            if (client != null && client.Connected) {
                client.GetStream().Close();
                client.Close(); // Supposed to handle stream as well
            }
            Environment.Exit(1);
        }

    }
}
