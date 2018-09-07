# Albot.Online C# Library

A simple library for communicating with the [Albot.Online](https://Albot.Online) client. 
This is great for getting you up and running fast, allowing you to focus more on the AI logic.
<br><br>
## Getting Started
This library is available as a NuGet package. [How do I install NuGet packages?](https://docs.microsoft.com/en-us/nuget/consume-packages/ways-to-install-a-package) 
<br>If you are using Visual Studio, you simply have to search for "Albot" in the Nuget Package Manager and click install.

## Example
Following is a short example of the C# Library being put to use on the [Snake](https://www.albot.online/snake/) game. 
For exact information of how to use the library see the [documentation Wiki](https://github.com/Albot-Online/Albot-CSharp-Library/wiki).

```cs
using System;
using System.Collections.Generic;

using Albot.Snake;

namespace SnakeBotDemo {
    class ProgramDemo {

        static void Main(string[] args) {
            SnakeGame game = new SnakeGame(); // Connects you to the client
            Random rnd = new Random();

            while (game.WaitForNextGameState() == BoardState.ongoing) { // Gets/Updates the board

                // Since this gives a struct with both playerMoves and enemyMoves, we specify playerMoves. 
                List<string> possibleMoves = game.GetPossibleMoves(game.currentBoard).playerMoves;

                int randomIndex = rnd.Next(possibleMoves.Count);
                string randomMove = possibleMoves[randomIndex];

                game.MakeMove(randomMove);
            }
        }
    }
}
```
This bot will simply connect to the client, look at what moves it currently has available and pick one at random.
<br><br>


## Versioning

  0.2b0
  
## Authors

  Joey Öhman

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/Albot-Online/Albot-CSharp-Library/blob/master/LICENSE) file for details
