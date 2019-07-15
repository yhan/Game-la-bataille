using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    /// Represent the card stack in front of each <see cref="Player"/>
    ///
    /// Take from the top of stack;
    /// Put back to the bottom of stack.
    /// 
    /// </summary>
    public class CardStack : IEnumerable<Card>
    {

        /// <summary>
        /// Top is at the end of list; bottom is at the beginning of list.
        /// </summary>
        private readonly List<Card> _inner;

        public CardStack(IEnumerable<Card> cartes)
        {
            _inner = cartes.ToList();
        }


        public CardStack Sort()
        {
            _inner.Sort();
            return this;
        }

        public CardStack Shuffle()
        {
            _inner.Shuffle();
            return this;
        }

        /// <summary>
        /// Take on card from the top.
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
        /// Put a card back to the bottom.
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

        public override string ToString()
        {
            return string.Join(", ", _inner);
        }
    }
}