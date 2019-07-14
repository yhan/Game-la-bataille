using System;
using System.Linq;

namespace LaBataille.Console
{
    /// <summary>
    /// input:
    ///  - un nombre de joueurs et un nombre de parties à jouer
    /// 
    /// En output:
    /// - L'historique des cartes ayant été vues sur le plateau
    /// - et le classement des joueurs à l'issue des parties
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Number of players? ");
            var numberOfPlayers = int.Parse( System.Console.ReadLine());

            System.Console.WriteLine("Number of rounds? ");
            var numberOfRounds = int.Parse( System.Console.ReadLine());

            var players = PlayersBuilder.BuildPlayers(numberOfPlayers).ToList();
            var gameFactory = new GameFactory(new CardsDistributor(CardsProvider.Instance, players));
            var competition = new Competition(numberOfRounds, gameFactory, players);





            var game = new Game(new CardsDistributor(new CardsProvider(), players));
            var gameOver = game.Play(NullShuffle.Instance);


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
            

            System.Console.ReadLine();
        }
    }
}
