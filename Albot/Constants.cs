namespace Albot {
    static class Constants {

        public static readonly int CONNECT4_BOARD_HEIGHT = 6;
        public static readonly int CONNECT4_BOARD_WIDTH = 7;

        public static class Fields {
            public const string board = "Board";
            public const string evaluate = "Evaluate";
            public const string possibleMoves = "PossMoves";
            public const string move = "Move";
            public const string player = "Player";
            public const string action = "Action";
            public const string winner = "Winner";
            public const string gameOver = "GameOver\n";
        }

        public static class Actions {
            public const string makeMove = "MakeMove";
            public const string simMove = "SimulateMove";
            public const string evalBoard = "EvaluateBoard";
            public const string getPossMoves = "GetPossibleMoves";
        }

    }
}
