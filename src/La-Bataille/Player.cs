using System.Collections.Generic;
using System.Linq;
using Value;

namespace La_Bataille
{
    public class Player : ValueType<Player>
    {
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
        }

        public CardStack CardStack { get; set; }

        public Take Lever(Visibility visibility)
        {
            var popped = CardStack.Pull();
            if (popped is NullCard)
            {
                return null;
            }
            return new Take(this, (Card)popped, visibility);
        }

        /// <summary>
        /// Player owns new <param name="cards"></param>
        /// </summary>
        /// <param name="cards"></param>
        public void Gather(IEnumerable<Card> cards)
        {
            foreach (var carte in cards)
            {
                CardStack.Carpet(carte);
            }
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Id };
        }
    }

    public static class JoueurExtensions
    {
        public static List<Take> TakeOneCardEach(this IEnumerable<Player> players,  Visibility visibility)
        {
            var takesWhenPossible = players.Select(j => j.Lever(visibility))
                .Where(l => l != null)
                .ToList();

            return takesWhenPossible;
        }
    }
}