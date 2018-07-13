using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using static Albot.Constants;

namespace Albot.GridBased {

    class GridBasedJsonHandler{

        private const string BOARD = "Board";

        internal static string CreateCommandPossibleMoves(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.getPossMoves),
                new JProperty(BOARD, board.Serialize())
                );
            return jsonCommand.ToString();
        }

        internal static string CreateCommandSimulateMove(GridBoard board, int player, int move) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.simMove),
                new JProperty(BOARD, board.Serialize()),
                new JProperty(Fields.player, player.ToString()),
                new JProperty(Fields.move, move)
                );
            return jsonCommand.ToString();
        }

        internal static string CreateCommandEvaluateBoard(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.evalBoard),
                new JProperty(BOARD, board.Serialize())
                );
            return jsonCommand.ToString();
        }

        internal static GridBoard ParseResponseState(string response, int width, int height) {
            JObject jResponse = JObject.Parse(response);
            //Console.WriteLine(jResponse.ToString());
            string serializedGrid = jResponse.GetValue(BOARD).ToString();
            //Console.WriteLine("Serialized grid: " + serializedGrid);
            return new GridBoard(width, height, serializedGrid);
        }

        // Optimize?
        internal static List<int> ParseResponsePossibleMoves(string response) {
            //Console.WriteLine(response);
            JObject jResponse = JObject.Parse(response);
            //Console.WriteLine(jResponse.ToString());
            string moves = jResponse.GetValue(Fields.possibleMoves).ToString();
            return JsonConvert.DeserializeObject<List<int>>(moves);
        }
        
        internal static BoardState ParseResponseEvaluateBoard(string response) {
            JObject jResponse = JObject.Parse(response);
            string boardState = jResponse.GetValue(Fields.boardState).ToString();
            return (BoardState)Enum.Parse(typeof(BoardState), boardState);
        }
    }
}
