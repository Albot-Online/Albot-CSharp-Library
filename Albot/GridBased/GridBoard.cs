using System;

namespace Albot.GridBased {
    public class GridBoard : IGameState {

        public readonly int WIDTH, HEIGHT;
        internal int[,] grid;

        public GridBoard(int width, int height, string serializedGrid) {
            WIDTH = width;
            HEIGHT = height;
            string[] cells = serializedGrid.TrimEnd().Split(' ');

            grid = new int[width, height];
            IterateBoard(
                (int x, int y) => {
                    grid[x, y] = int.Parse(cells[x + y * 7]);
                }
            );
        }

        // Deep copy
        public GridBoard(int width, int height, int[,] grid) {
            WIDTH = width;
            HEIGHT = height;
            this.grid = (int[,])grid.Clone();
        }
        // Deep copy
        public GridBoard(GridBoard gridBoard) {
            WIDTH = gridBoard.WIDTH;
            HEIGHT = gridBoard.HEIGHT;
            this.grid = (int[,])gridBoard.grid.Clone();
        }

        /// <summary>
        /// Gives the value of a cell. (0,0) is in the top left corner.
        /// </summary>
        /// <param name="x">Zero based x index</param>
        /// <param name="y">Zero based y index</param>
        /// <returns>1 if player, -1 if enemy, 0 if empty</returns>
        public int GetCell(int x, int y) {
            return grid[x, y];
        }

        public string Serialize() {
            string boardString = "";

            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    boardString += grid[x, y] + " ";

            return boardString.Remove(boardString.Length - 1);
        }

        #region Util
        private void IterateBoard(Action<int, int> cellFunc) {
            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    cellFunc(x, y);
        }

        private void IterateBoard(Action<int, int> cellFunc, Action<int> rowFunc) {
            for (int y = 0; y < 6; y++) {
                for (int x = 0; x < 7; x++)
                    cellFunc(x, y);

                rowFunc(y);
            }
        }
        #endregion

        #region Debugging
        public override string ToString() {
            string boardString = "";

            IterateBoard(
                (int x, int y) => { boardString += grid[x, y] + "\t"; },
                (int y) => {
                    boardString += "\n";
                }
            );

            return boardString;
        }

        public void PrintBoard() { Console.WriteLine(this.ToString()); }
        #endregion


    }
}
