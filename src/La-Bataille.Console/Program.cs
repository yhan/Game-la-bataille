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
            while (true)
            {
                System.Console.WriteLine("Number of players? ");
                var numberOfPlayers = int.Parse( System.Console.ReadLine());

                System.Console.WriteLine("Number of rounds? ");
                var numberOfRounds = int.Parse( System.Console.ReadLine());

                var players = PlayersBuilder.BuildPlayers(numberOfPlayers).ToList();
                var gameFactory = new GameFactory(new CardsDistributor(CardsProvider.Instance, players));
                var competition = new Competition(numberOfRounds, gameFactory, players);


                var gameOverVisitor = new ConsoleVisitGameOver();
                var ranking = competition.Play(gameOverVisitor);

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("###########   RANKING   #####################");
                System.Console.ResetColor();
            
                foreach (var rank in ranking)
                {
                    System.Console.WriteLine($"RANK: {rank.Number} | Player: '{rank.Player.Id}' | Score: {rank.Score}");
                }

                System.Console.WriteLine("Continue? y/n");
                if (System.Console.ReadKey().Key == ConsoleKey.N)
                {
                    break;
                }
            }

            System.Console.ReadLine();
        }
    }
}
