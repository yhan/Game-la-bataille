using System;
using System.Linq;

namespace LaBataille.Console
{
    /// <summary>
    /// [INPUT]:
    /// - a number of players and a number of games to play
    ///
    /// [OUTPUT]:
    /// - The history of the cards having been seen on the board
    /// - and the ranking of the players after the games
    ///
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Ranking ranking;
                try
                {
                    System.Console.WriteLine();

                    System.Console.WriteLine("Number of players? ");
                    var numberOfPlayers = int.Parse( System.Console.ReadLine());

                    System.Console.WriteLine("Number of rounds? ");
                    var numberOfRounds = int.Parse( System.Console.ReadLine());

                    var players = PlayersBuilder.BuildPlayers(numberOfPlayers, new RealCardsShuffler()).ToList();
                    var gameFactory = new GameFactory(new CardsDistributor(CardsProvider.Instance, players));
                    var competition = new Competition(numberOfRounds, gameFactory, players);
                    var gameOverVisitor = new ConsoleVisitGameOver();
                    ranking = competition.Play(gameOverVisitor);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }
                

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("\r\n\r\n###########   RANKING   #####################");
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
