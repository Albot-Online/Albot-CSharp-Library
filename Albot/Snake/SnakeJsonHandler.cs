using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static Albot.Snake.SnakeStructs;
using static Albot.Snake.SnakeConstants;

using static Albot.Constants;

namespace Albot.Snake {

    internal static class SnakeJsonHandler {
        private static JsonSerializer serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

        private static JObject SerializeBoard(SnakeBoard board) {
            JObject jObject = new JObject(
                new JProperty(JProtocol.player,
                    JObject.FromObject(board.player)),
                new JProperty(JProtocol.enemy,
                    JObject.FromObject(board.enemy))
                );
            //if (includeBlocked)
            jObject.Add(new JProperty(JProtocol.blocked, JArray.FromObject(board.GetBlockedList(false))));
            return jObject;
        }

        #region CreateCommand
        /*
        internal static string CreateDirectionCommand(string direction) {
            JObject jsonCommand = new JObject(
                new JProperty(JProtocol.dir, direction)
                );
            Console.WriteLine("Direction Command: \n" + jsonCommand + "\n");
            return jsonCommand.ToString();
        }
        */

        internal static string CreateCommandPossibleMoves(SnakeBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Constants.Fields.action, Actions.getPossMoves),
                new JProperty(JProtocol.player, board.GetPlayerDirection()),
                new JProperty(JProtocol.enemy, board.GetEnemyDirection())
            );
            //Console.WriteLine("PossibleMoves Command: \n" + jsonCommand + "\n");

            return jsonCommand.ToString();
        }

        internal static string CreateCommandSimulate(SnakeBoard board, MovesToSimulate simMoves) {
            JObject jsonCommand = new JObject(
                new JProperty(Constants.Fields.action, Actions.simMove),
                JObject.FromObject(simMoves, serializer).Children(),
                new JProperty(JProtocol.board, SerializeBoard(board))
                );
            //Console.WriteLine("Simulate Command: \n" + jsonCommand + "\n");

            return jsonCommand.ToString();
            //return FixVariableNamings(jsonCommand.ToString());
        }

        internal static string CreateCommandEvaluate(SnakeBoard board) {
            JObject jsonCommand = new JObject(
                new JProperty(Constants.Fields.action, Actions.evalBoard),
                new JProperty(JProtocol.board, SerializeBoard(board))
            );
            //Console.WriteLine("Evaluate command: \n" + jsonCommand + "\n");
            return jsonCommand.ToString();
        }
        
        #endregion

        #region ParseResponse
        internal static BoardStruct ParseResponseState(JObject jState) {
            //Console.WriteLine("Response state: " + "\n" + jResponse.ToString());
            return jState.ToObject<BoardStruct>();
        }
        
        internal static PossibleMoves ParseResponsePossibleMoves(string response) {
            PossibleMoves possMoves = JsonHandler.TryDeserialize<PossibleMoves>(response);
            //Console.WriteLine("Response possMoves: " + "\n" + JObject.Parse(response).ToString());
            return possMoves;
        }
        
        internal static SnakeBoard ParseResponseSimulate(string response) {
            //Console.WriteLine(response);
            JObject jBoard = JsonHandler.TryParse(response);
            //Console.WriteLine("Response simulate: \n" + jBoard.ToString());
            return new SnakeBoard(jBoard.ToObject<BoardStruct>());
        }

        internal static BoardState ParseResponseEvaluate(string response) {
            JObject jState = JsonHandler.TryParse(response);
            //Console.WriteLine("Response evaluate: \n" + jState + "\n");
            return JsonHandler.FetchBoardState(jState);
        }

        #endregion

    }
}
