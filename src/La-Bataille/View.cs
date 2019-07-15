using System.Collections;
using System.Collections.Generic;

namespace LaBataille
{
    /// <summary>
    /// Represent what we see on the table for a given time.
    /// </summary>
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

    /// <summary>
    /// Represent a card is played by a <see cref="Player"/> wih Face-Up or Face-down property (Cf.<see cref="Visibility"/>).
    /// </summary>
    public struct TwoFaceCard
    {
        public Card Card { get; }
        public Visibility Visibility { get; }
        public int PlayerId { get; }

        public TwoFaceCard(Player player, Card card, Visibility visibility)
        {
            Card = card;
            Visibility = visibility;
            PlayerId = player.Id;
        }

        public override string ToString()
        {
            return $"{PlayerId}: {Card}({Visibility})";
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