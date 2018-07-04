using System;
using System.Collections.Generic;
using System.Text;

using static Albot.Snake.SnakeConstants.Fields;
using static Albot.Snake.SnakeStructs;


namespace Albot.Snake {
    public class SnakeBoard {
        // Optimize? (creating new state every update)

        internal Placement playerPlacement, enemyPlacement;
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
            playerPlacement = response.player;
            enemyPlacement = response.enemy;
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
            if ((x == playerPlacement.x && y == playerPlacement.y) || (x == enemyPlacement.x && y == enemyPlacement.y))
                return true;
            return blocked[x, y];
        }

        public Position GetPlayerPosition() { return new Position() { x = playerPlacement.x, y = playerPlacement.y }; }
        public Position GetEnemyPosition() { return new Position() { x = enemyPlacement.x, y = enemyPlacement.y }; }
        public string GetPlayerDirection() { return playerPlacement.dir; }
        public string GetEnemyDirection() { return enemyPlacement.dir; }

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
        private string SquareInfo(int x, int y) {
            if (x == playerPlacement.x && y == playerPlacement.y)
                return "P";
            if (x == enemyPlacement.x && y == enemyPlacement.y)
                return "E";
            return Convert.ToInt32(blocked[x, y]) + "";
        }
        #endregion

    }
}
