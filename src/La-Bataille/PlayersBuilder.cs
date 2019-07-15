using System;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    public class PlayersBuilder
    {
        public static IEnumerable<Player> BuildPlayers(int count, IShuffleCards cardsShuffler)
        {
            if (count < 2)
            {
                throw new ArgumentException("Should have at least 2 players in a game");
            }

            IEnumerable<Player> players = Enumerable.Range(0, count: count).Select(id => new Player(id, cardsShuffler)).ToList();
            return players;
        }
    }
}