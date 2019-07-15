using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    public static class PlayerExtensions
    {
        public static List<Take> TakeOneCardEach(this IEnumerable<Player> players,  Visibility visibility)
        {
            var takesWhenPossible = players.Select(j => j.Take(visibility))
                .Where(l => l != null)
                .ToList();

            return takesWhenPossible;
        }
        
        public static Player[] WhoHaveCards(this IEnumerable<Player> players)
        {
            return players.Where(c => c.HasCards()).ToArray();
        }
    }
}