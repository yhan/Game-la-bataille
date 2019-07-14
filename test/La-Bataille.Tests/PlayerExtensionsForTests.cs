using System.Collections.Generic;

namespace LaBataille.Tests
{
    public static class PlayerExtensionsForTests
    {
        public static List<Player> With(this List<Player> players, List<List<Card>> cards)
        {
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                player.CardStack = new CardStack(cards[i]);
            }

            return players;
        }
    }
}