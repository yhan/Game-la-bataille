using System;

namespace LaBataille.Console
{
    public class ConsoleVisitGameOver : IVisitGameOver
    {
        public void Visit(Game game, IAmTheGameOver gameOver)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("###########   HISTORY OF TABLE VIEWS   #####################");
            System.Console.ResetColor(); 
            foreach (var view in game.TableViewsHistory)
            {
                System.Console.WriteLine(view);
            }
            
            System.Console.ForegroundColor = ConsoleColor.Green;

            switch (gameOver)
            {
                case Draw _:
                    System.Console.WriteLine("GAME ENDED WITH A DRAW");
                    break;
                case HasWinner hasWinner:
                    var winner = hasWinner.Winner;
                    System.Diagnostics.Debug.Assert(winner.CardStack.Size + game.DroppedCards.Count == 52);

                    System.Console.WriteLine($"GAME OVER.");
                    System.Console.WriteLine($"THE WINNER IS 'Player {hasWinner.Winner.Id}'");
                    break;
            }
        }
    }
}