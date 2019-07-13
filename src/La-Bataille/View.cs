using System.Collections;
using System.Collections.Generic;

namespace La_Bataille
{
    public class View : IEnumerable<TwoFaceCard>
    {
        private readonly IEnumerable<TwoFaceCard> _cards;

        public View(IEnumerable<TwoFaceCard> cards)
        {
            _cards = cards;
        }

        public IEnumerator<TwoFaceCard> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", _cards);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public struct TwoFaceCard
    {
        public Card Card { get; }
        public Visibility Visibility { get; }

        public TwoFaceCard(/*Player takePlayer,*/ Card card, Visibility visibility)
        {
            Card = card;
            Visibility = visibility;
        }

        public override string ToString()
        {
            return $"{Card}({Visibility})";
        }
    }

    /// <summary>
    /// The visibility of card on the table
    /// </summary>
    public enum Visibility
    {
        FaceUp,
        FaceDown
    }
}