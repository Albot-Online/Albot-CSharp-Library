namespace Albot.Connect4 {
    public class Connect4Game : GridBasedGame {
        
        public Connect4Game(string ip = "127.0.0.1", int port = 4000) : base(ip,port) {
            
        }

        protected override void InitGridDimensions() {
            this.width = Connect4Constants.Fields.boardWidth;
            this.height = Connect4Constants.Fields.boardHeight;
        }

        public Connect4Board MakeMoveGetNextBoard(int move) {
            MakeMove(move);
            return GetNextBoard();
        }

        public new Connect4Board GetNextBoard() {
            return new Connect4Board(base.GetNextBoard());
        }

        public Connect4Board SimulateMove(Connect4Board board, int player, int move) {
            GridBoard gridBoard = base.SimulateMove(board, player, move);
            return new Connect4Board(gridBoard);
        }
        
    }
}
