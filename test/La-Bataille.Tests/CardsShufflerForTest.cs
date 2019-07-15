using System.Collections.Generic;
using System.Linq;

namespace LaBataille.Tests
{
    /// <summary>
    /// Put the smaller one on the bottom of CardStack, 
    /// to introduce some determinism for testing.
    /// </summary>
    public class CardsShufflerForTest : IShuffleCards
    {
        public IEnumerable<Card> Shuffle(IEnumerable<Card> cards)
        {
            return cards.OrderByDescending(card => card);
        }
    }
}