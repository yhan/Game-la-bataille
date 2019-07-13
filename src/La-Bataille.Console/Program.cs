﻿namespace La_Bataille.Console
{
    using System;
    
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
            Console.WriteLine("Number of players? ");
            var numberOfPlayers = int.Parse( Console.ReadLine());

            var players = PlayersBuilder.BuildPlayers(numberOfPlayers);

            var game = new Game(new CardsDistributor(new CardsProvider(), players));
            var gameOver = game.Play(NullShuffle.Instance);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("###########   HISTORY OF TABLE VIEWS   #####################");
            Console.ResetColor(); 
            foreach (var view in game.TableViewsHistory)
            {
                Console.WriteLine(view);
            }
            
            Console.ForegroundColor = ConsoleColor.Green;

            switch (gameOver)
            {
                case Draw _:
                    Console.WriteLine("GAME ENDED WITH A DRAW");
                    break;
                case HasWinner hasWinner:
                    var winner = hasWinner.Winner;
                    System.Diagnostics.Debug.Assert(winner.CardStack.Size + game.DroppedCards.Count == 52);

                    Console.WriteLine($"GAME OVER.");
                    Console.WriteLine($"THE WINNER IS 'Player {hasWinner.Winner.Id}'");
                    break;
            }
            

            Console.ReadLine();
        }
    }
}
