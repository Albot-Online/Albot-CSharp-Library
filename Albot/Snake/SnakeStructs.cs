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

            public override string ToString() {
                return "{'x': " + x + ", 'y': " + y + "}";
            }
        }

        public struct PossibleMoves {
            public List<string> playerMoves, enemyMoves;
        }

        public struct BoardStruct {
            public Placement player, enemy;
            public List<Position> blocked;
        }

        public struct MovesToSimulate {
            public string playerMove, enemyMove;
        }
    }
}
