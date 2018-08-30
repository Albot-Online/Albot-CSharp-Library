using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using static Albot.Constants;

namespace Albot.GridBased {

    class GridBasedJsonHandler{

        #region CreateCommand
        internal static string CreateCommandPossibleMoves(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.getPossMoves),
                //new JProperty(Fields.board, board.Serialize())
                new JProperty(Fields.board, JArray.FromObject(GridBoard.Transpose(board.grid, board.WIDTH, board.HEIGHT)))
                );
            //Console.WriteLine("Command possmoves: \n" + jsonCommand.ToString() + "\n");
            return jsonCommand.ToString();
        }

        internal static string CreateCommandSimulateMove(GridBoard board, int player, int move) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.simMove),
                //new JProperty(Fields.board, board.Serialize()),
                new JProperty(Fields.board, JArray.FromObject(GridBoard.Transpose(board.grid, board.WIDTH, board.HEIGHT))),
                new JProperty(Fields.player, player),
                new JProperty(Fields.move, move)
                );
            //Console.WriteLine("Command simMove: \n" + jsonCommand.ToString() + "\n");
            return jsonCommand.ToString();
        }

        internal static string CreateCommandEvaluateBoard(GridBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Fields.action, Actions.evalBoard),
                //new JProperty(Fields.board, board.Serialize())
                new JProperty(Fields.board, JArray.FromObject(GridBoard.Transpose(board.grid, board.WIDTH, board.HEIGHT)))
                );
            //Console.WriteLine("Command evaluate: \n" + jsonCommand.ToString() + "\n");
            return jsonCommand.ToString();
        }
        #endregion

        #region ParseResponse
        internal static GridBoard ParseResponseState(JObject jState, int width, int height) {

            return ParseJsonGridBoard(jState, width, height);
        }

        // Optimize?
        internal static List<int> ParseResponsePossibleMoves(string response) {
            JObject jResponse = JsonHandler.TryParse(response);
            //Console.WriteLine("Response possmoves: \n" + jResponse.ToString() + "\n");
            string moves = jResponse.GetValue(Fields.possibleMoves).ToString();
            return JsonConvert.DeserializeObject<List<int>>(moves);
        }

        internal static GridBoard ParseResponseSimulateMove(string response, int width, int height) {
            JObject jState = JsonHandler.TryParse(response);
            return ParseJsonGridBoard(jState, width, height);
        }
        
        internal static BoardState ParseResponseEvaluateBoard(string response) {
            JObject jResponse = JsonHandler.TryParse(response);
            return JsonHandler.FetchBoardState(jResponse);
        }

        private static GridBoard ParseJsonGridBoard(JObject jState, int width, int height) {
            string gridString = jState.GetValue(Fields.board).ToString();
            int[,] grid = JsonConvert.DeserializeObject<int[,]>(gridString);
            grid = GridBoard.Transpose(grid, height, width); // Received as [y, x], we want [x, y]
            return new GridBoard(width, height, grid);
        }
        #endregion
    }
}
