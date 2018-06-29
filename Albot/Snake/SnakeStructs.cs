using System;
using System.Collections.Generic;
using System.Text;

namespace Albot.Snake {

    public class SnakeStructs {
        
        public struct Placement {
            //public Position Pos;
            public int x;
            public int y;

            public string dir;
        }
        
        public struct Position {
            public int x;
            public int y;
        }

        public struct PossibleMoves {
            public List<string> player;
            public List<string> enemy;
        }

        public struct BoardStruct {
            public Placement player, enemy;
            public List<Position> blocked;
        }

        public struct PossibleMovesResponse {
            public List<string> playerMoves, enemyMoves;
        }

        public struct MovesToSimulate {
            public string playerMove, enemyMove;
        }
    }
}
