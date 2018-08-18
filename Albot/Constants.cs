namespace Albot {

    /// <summary>
    /// Whether game is over and if so, who the winner is.
    /// </summary>
    public enum BoardState { playerWon, enemyWon, draw, ongoing }

    public static class Constants {
        
        public static class Fields {
            public const string board = "board";
            public const string evaluate = "evaluate";
            public const string possibleMoves = "possMoves";
            public const string move = "move";
            public const string player = "player";
            public const string action = "action";
            public const string winner = "winner";
            public const string gameOver = "gameOver";
            public const string boardState = "boardState";
        }

        public static class Actions {
            public const string restartGame = "restartGame";
            public const string makeMove = "makeMove";
            public const string simMove = "simulateMove";
            public const string evalBoard = "evaluateBoard";
            public const string getPossMoves = "getPossibleMoves";
        }

    }
}
