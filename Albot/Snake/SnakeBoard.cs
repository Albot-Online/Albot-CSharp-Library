using System;
using System.Collections.Generic;
using System.Text;

using static Albot.Snake.SnakeConstants.Fields;
using static Albot.Snake.SnakeStructs;


namespace Albot.Snake {
    public class SnakeBoard {
        // Optimize? (creating new state every update)

        /// <summary>
        /// The placements of the players contain their positions and directions.
        /// </summary>
        public Placement player, enemy;
        private bool[,] blocked = new bool[boardWidth, boardHeight];

        public SnakeBoard(SnakeBoard board, BoardStruct boardStruct) {
            ExtractOldBoardInfo(board);
            ExtractResponseInfo(boardStruct);
        }
        public SnakeBoard(BoardStruct boardStruct) {
            ExtractResponseInfo(boardStruct);
        }
        // Add constructors for users?

        private void ExtractResponseInfo(BoardStruct response) {
            player = response.player;
            enemy = response.enemy;
            /* Does not work well with Evaluate
            if(CoordsInBounds(playerPlacement.x, playerPlacement.y))
                blocked[playerPlacement.x, playerPlacement.y] = true;
            if(CoordsInBounds(enemyPlacement.x, enemyPlacement.y))
                blocked[enemyPlacement.x, enemyPlacement.y] = true;
            */

            foreach (Position pos in response.blocked) {
                if(CoordsInBounds(pos.x, pos.y))
                    blocked[pos.x, pos.y] = true;
            }
        }

        private void ExtractOldBoardInfo(SnakeBoard board) {
            if (board != null)
                for (int x = 0; x < boardWidth; x++)
                    for (int y = 0; y < boardHeight; y++)
                        blocked[x, y] = board.blocked[x, y];
        }

        /// <summary>
        /// True if position is occupied, false if square is empty.
        /// </summary>
        public bool CellBlocked(int x, int y) {
            if (x < 0 || y < 0 || x >= boardWidth || y >= boardHeight)
                return true; // Out of bounds
            if ((x == player.x && y == player.y) || (x == enemy.x && y == enemy.y))
                return true;
            return blocked[x, y];
        }

        public Position GetPlayerPosition() { return new Position() { x = player.x, y = player.y }; }
        public Position GetEnemyPosition() { return new Position() { x = enemy.x, y = enemy.y }; }
        public string GetPlayerDirection() { return player.dir; }
        public string GetEnemyDirection() { return enemy.dir; }

        /// <summary>
        /// Returns a list of occupied positions.
        /// </summary>
        public List<Position> GetBlockedList(bool includePlayerPositions = true) {
            List<Position> b = new List<Position>();
            for (int xb = 0; xb < boardWidth; xb++)
                for (int yb = 0; yb < boardHeight; yb++)
                    if (blocked[xb, yb])
                        b.Add(new Position() { x = xb, y = yb });
            if (includePlayerPositions) {
                b.Add(GetPlayerPosition());
                b.Add(GetEnemyPosition());
            }
            return b;
        }

        private bool CoordsInBounds(int x, int y) {
            if (x < 0 || y < 0 || x >= boardWidth || y >= boardHeight)
                return false;
            return true;
        }

        #region debug
        public override string ToString() {
            string s = "";
            for (int y = 0; y < boardHeight; y++) {
                for (int x = 0; x < boardWidth; x++)
                    s += SquareInfo(x, y) + " ";
                s += "\n";
            }
            return s;
        }

        /// <summary>
        /// Prints the board to the console. 
        /// </summary>
        /// <param name="boardName">Optional title for the printed board.</param>
        public void PrintBoard(string boardName = "") {
            Console.WriteLine("* * * * * *" + boardName + "* * * * * *");
            Console.Write(ToString());
            Console.WriteLine("* * * * * * * * * * * *");
        }
        private string SquareInfo(int x, int y) {
            if (x == player.x && y == player.y)
                return "P";
            if (x == enemy.x && y == enemy.y)
                return "E";
            return blocked[x, y] ? "X" : "0";
        }
        #endregion

    }
}
