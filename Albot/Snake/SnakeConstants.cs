using System;
using System.Collections.Generic;
using System.Text;

namespace Albot.Snake {
    public static class SnakeConstants {

        public static class Fields {
            public const int boardWidth = 10;
            public const int boardHeight = 10;

            public const string right = "right";
            public const string up = "up";
            public const string left = "left";
            public const string down = "down";
        }

        public static class JProtocol {
            public const string board = "board";
            public const string posX = "x";
            public const string posY = "y";
            public const string dir = "dir";
            public const string player = "player";
            public const string enemy = "enemy";
            public const string blocked = "blocked";
            
            public const string playerMove = "playerMove";
            public const string enemyMove = "enemyMove";
            public const string playerMoves = "playerMoves";
            public const string enemyMoves = "enemyMoves";
            
        }

    }
}
