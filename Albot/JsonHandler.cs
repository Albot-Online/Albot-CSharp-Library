using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Albot {

    class JsonHandler {

        internal static JObject TryParse(string response) {
            try {
                return JObject.Parse(response);
            } catch (Exception e) {
                Console.WriteLine("Could not parse response: \n" + response + "\n" + e);
                AlbotConnection.Terminate();
            }
            return null; // Unreachable
        }

        internal static T TryDeserialize<T>(string response) {
            try {
                return JsonConvert.DeserializeObject<T>(response);
            } catch (Exception e) {
                Console.WriteLine("Could not deserialize response: \n" + response + "\n" + e);
                AlbotConnection.Terminate();
            }
            return default(T); // Unreachable
        }

        internal static BoardState ExtractBoardState(ref JObject stateResponse) {
            string boardState = stateResponse.GetValue(Constants.Fields.boardState).ToObject<string>();
            stateResponse.Remove(Constants.Fields.boardState);
            return (BoardState)Enum.Parse(typeof(BoardState), boardState);
        }

        internal static BoardState FetchBoardState(JObject stateResponse) {
            string boardState = stateResponse.GetValue(Constants.Fields.boardState).ToObject<string>();
            return (BoardState)Enum.Parse(typeof(BoardState), boardState);
        }
    }
}
