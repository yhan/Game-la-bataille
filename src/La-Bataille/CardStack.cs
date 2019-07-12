using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    /// <summary>
    /// Represent the card stack in front of each <see cref="Player"/>
    /// </summary>
    public class CardStack : IEnumerable<Card>
    {
        private readonly List<Card> _inner;

        public CardStack(IEnumerable<Card> cartes)
        {
            _inner = cartes.ToList();
        }

        /// <summary>
        /// Take on card
        /// </summary>
        /// <returns></returns>
        public IAmCard Pull()
        {
            if (_inner.Count == 0)
            {
                return NullCard.Instance;
            }

            var last = _inner.Count - 1;
            var popped = _inner[last];
            _inner.RemoveAt(last);

            return popped;
        }

        /// <summary>
        /// Put a card back to the bottom of the <see cref="CardStack"/>
        /// </summary>
        /// <param name="card"></param>
        public void Carpet(Card card)
        {
            _inner.Insert(0, card);
        }

        /// <summary>
        /// Number of cards in the <see cref="CardStack"/>
        /// </summary>
        public int Size => _inner.Count;

        public IEnumerator<Card> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}