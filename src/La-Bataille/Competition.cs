using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LaBataille
{
    public class Competition
    {
        private readonly List<Player> _players;
        private readonly List<Game> _games = new List<Game>();

        public Competition(int numberOfRounds, IMakeGames factory, List<Player> players)
        {
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
                var gameOver = game.Play(NullShuffle.Instance);

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

    public interface IVisitGameOver
    {
        void Visit(Game game, IAmTheGameOver gameOver);
    }
}