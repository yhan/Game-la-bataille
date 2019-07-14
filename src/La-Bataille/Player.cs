using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace LaBataille
{
    public class Player : ValueType<Player>
    {
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
        }

        public CardStack CardStack { get; set; }
        public Score Score { get; } = new Score(0);

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

        public void Gather(IEnumerable<Take> takes)
        {
            this.Gather(takes.Select(x => x.Card)
                .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Id };
        }

        public override string ToString()
        {
            return $"{Id}: {CardStack}";
        }

        public void Scores(int score)
        {
            Score.Increment(score);
        }
    }
}