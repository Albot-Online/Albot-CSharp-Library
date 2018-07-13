namespace Albot {

    /// <summary>
    /// Whether game is over and if so, who the winner is.
    /// </summary>
    public enum BoardState { PlayerWon, EnemyWon, Draw, Ongoing }

    public static class Constants {
        
        public static class Fields {
            public const string board = "board";
            public const string evaluate = "Evaluate";
            public const string possibleMoves = "PossMoves";
            public const string move = "Move";
            public const string player = "Player";
            public const string action = "Action";
            public const string winner = "Winner";
            public const string gameOver = "GameOver";//\n";
            public const string boardState = "boardState";
        }

        public static class Actions {
            public const string restartGame = "RestartGame";
            public const string makeMove = "MakeMove";
            public const string simMove = "SimulateMove";
            public const string evalBoard = "EvaluateBoard";
            public const string getPossMoves = "GetPossibleMoves";
        }

    }
}
