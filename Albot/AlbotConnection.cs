using System;
using System.Text;
using System.Net.Sockets;

namespace Albot {

    public class AlbotConnection {

        private NetworkStream stream;
        private string state;
        private bool stateUpToDate = false;
        
        public AlbotConnection(string ip = "127.0.0.1", int port = 4000) { 

            TcpClient client = new TcpClient(ip, port);
            stream = client.GetStream();
        }

        
        public string GetState() {
            if (stateUpToDate)
                return state;

            state = ReceiveState();
            stateUpToDate = true;
            return state;
        }

        // Blocking receive for new state
        private string ReceiveState() {
            Byte[] data = new byte[256];

            Int32 bytes = stream.Read(data, 0, data.Length);

            string incomingData;
            do {
                incomingData = Encoding.Default.GetString(data, 0, bytes);

            } while (incomingData == null);// Blocking receive

            return incomingData;
        }

        // Make move, should not be blocking? (It should be async)
        //public void SendCommand(int move) {SendCommand(move.ToString());}
        public void SendCommand(string jsonCommand) {
            stateUpToDate = false;
            byte[] response = Encoding.Default.GetBytes(jsonCommand);
            stream.Write(response, 0, response.Length);
        }

        //public string SendCommandRecvState(int command) { return SendCommandRecvState(command.ToString()); }
        public string SendCommandRecvState(string jsonCommand) {
            SendCommand(jsonCommand);
            return GetState();
        }

    }
}
