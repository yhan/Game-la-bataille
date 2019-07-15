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

        public static bool OnlyOneStillHasCards(this IEnumerable<Player> players, out Player winner)
        {
            var survivors = players.Where(x => x.CardStack.Size > 0);
            
            var survivorsArray = survivors as Player[] ?? survivors.ToArray();

            if (survivorsArray.Length == 1)
            {
                winner = survivorsArray.Single();
                return true;
            }

            if (survivorsArray.Length == 0)
            {
                winner = null;
                return false;
            }
            

            winner = null;
            return false;

        }

        public static bool NobodyHasCards(this IEnumerable<Player> players)
        {
            return players.All(p => p.CardStack.Size == 0);
        }


        public static bool AllButOneAreEmpty(this IEnumerable<Player> players, out Player survivor)
        {
            var survivors = players.Where(p=> p.HasCards()).ToArray();
            if (survivors.Length > 1)
            {
                survivor = null;
                return false;
            }

            survivor = survivors.Single();
            return true;
        }
    }

    
}