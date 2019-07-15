using System;
using System.Linq;

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
                case Draw draw:
                    System.Console.WriteLine($"GAME ENDED WITH A DRAW. Reason: {draw.Reason}");

                    PrintDroppedCards(game);

                    System.Console.WriteLine("Players still have cards: ");
                    foreach (var player in game.Players.Where(p => p.HasCards()))
                    {
                        System.Console.WriteLine($"Player {player.Id} having {player.CardStack.Size} cards");
                        System.Console.WriteLine($"    ===> They are: {player.CardStack.Sort()}");
                    }

                    break;
                case HasWinner hasWinner:
                    var winner = hasWinner.Winner;
                    System.Diagnostics.Debug.Assert(winner.CardStack.Size + game.DroppedCards.Count == game.DistributedCardsSize);

                    System.Console.WriteLine($"GAME OVER.");
                    
                    System.Console.WriteLine($"THE WINNER IS 'Player {winner.Id}'");
                    System.Console.WriteLine($"    ===> He has {winner.CardStack.Size} cards: {winner.CardStack.Sort()}");

                    PrintDroppedCards(game);

                    break;
            }
        }

        private static void PrintDroppedCards(Game game)
        {
            System.Console.WriteLine($"Number of dropped cards: {game.DroppedCards.Count}");
            System.Console.WriteLine($"They are: {string.Join(", ", game.DroppedCards)}");
        }
    }
}