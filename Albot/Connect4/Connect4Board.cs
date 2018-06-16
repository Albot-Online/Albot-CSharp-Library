namespace Albot.Connect4 {
    public class Connect4Board : GridBoard {

        public Connect4Board(string input) : base(7, 6, input) {

        }

        public Connect4Board(int[,] grid) : base(7, 6, grid) {

        }

        public Connect4Board(Connect4Board b) : base(7, 6, b.grid) {

        }

        public Connect4Board(GridBoard gb) : base(7, 6, gb.grid) {

        }
    }
}
