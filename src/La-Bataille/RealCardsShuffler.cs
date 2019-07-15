using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    
    public interface IShuffleCards
    {
        IEnumerable<Card> Shuffle(IEnumerable<Card> cards);
    }

    public class RealCardsShuffler : IShuffleCards
    {
        public IEnumerable<Card> Shuffle(IEnumerable<Card> cards)
        {
            return cards.ShuffleList();
        }
    }
}