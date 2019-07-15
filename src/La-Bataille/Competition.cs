using System;
using System.Collections.Generic;

namespace LaBataille
{

    /// <summary>
    /// A competition is composed of N <see cref="Game"/> (N >= 1), also called `Rounds`
    /// </summary>
    public class Competition
    {
        private readonly List<Player> _players;
        private readonly List<Game> _games = new List<Game>();

        public Competition(int numberOfRounds, GameFactory factory, List<Player> players)
        {
            if (numberOfRounds < 1)
            {
                throw new ArgumentException("A competition should at least one round.");
            }

            _players = players;
            
            for (int round = 0; round < numberOfRounds; round++)
            {
                _games.Add(factory.Build());
            }
        }
        
        public Ranking Play(IVisitGameOver gameOverVisitor)
        {
            foreach (var game in _games)
            {
                var gameOver = game.Play();

                gameOverVisitor.Visit(game, gameOver);

                switch (gameOver)
                {
                    case Draw _:
                        foreach (var player in game.Players)
                        {
                            player.Scores(1);
                        }

                        break;

                    case HasWinner hasWinner:
                        var winner = hasWinner.Winner;
                        winner.Scores(3);

                        break;
                }
            }
            
            return new Ranking(_players);
        }
    }
}