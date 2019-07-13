using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class PlayersBuilder
    {
        public static IEnumerable<Player> BuildPlayers(int count)
        {
            IEnumerable<Player> players = Enumerable.Range(0, count: count).Select(x => new Player(x)).ToList();
            return players;
        }
    }
}