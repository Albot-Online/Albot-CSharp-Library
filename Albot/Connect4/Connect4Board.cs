using static Albot.Connect4.Connect4Constants.Fields;

namespace Albot.Connect4 {
    public class Connect4Board : GridBoard {

        public Connect4Board(string input) : base(boardWidth, boardHeight, input) {

        }

        public Connect4Board(int[,] grid) : base(boardWidth, boardHeight, grid) {

        }

        public Connect4Board(Connect4Board b) : base(boardWidth, boardHeight, b.grid) {

        }

        public Connect4Board(GridBoard gb) : base(boardWidth, boardHeight, gb.grid) {

        }
    }
}
