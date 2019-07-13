using System.Collections.Generic;
using La_Bataille;

namespace Tests
{
    public class TestGamesProvider : IMakeGames
    {
        private readonly int _numberOfGames;
        private readonly IDistributeCards _distributor;

        public TestGamesProvider(IEnumerable<Player> players, int numberOfGames, IDistributeCards distributor)
        {
            Players = players;
            _numberOfGames = numberOfGames;
            _distributor = distributor;
        }

        public IEnumerable<Game> Build()
        {
            for (int i = 0; i < _numberOfGames; i++)
            {
                yield return new Game(_distributor);
            }
        }

        public IEnumerable<Player> Players { get; }
    }
}