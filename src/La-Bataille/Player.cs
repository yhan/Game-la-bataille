using System;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    /// The participant of <see cref="Game"/>
    /// </summary>
    public class Player
    {
        private readonly IShuffleCards _cardsShuffler;
        public int Id { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <param name="cardsShuffler">Used for shuffling earned cards</param>
        public Player(int id, IShuffleCards cardsShuffler)
        {
            _cardsShuffler = cardsShuffler;
            Id = id;
        }

        /// <summary>
        /// The card stack (all cards face down) in front of all players
        /// </summary>
        public CardStack CardStack { get; set; }

        /// <summary>
        /// The scored earned by the player
        /// </summary>
        public Score Score { get; } = new Score(0);
        
        /// <summary>
        /// The player take a card, with two possible card visibility: face up or face down.
        /// </summary>
        /// <param name="visibility">Face up or Face down</param>
        /// <returns>The card taken with its associated properties: which player, what card visibility ...</returns>
        public Take Take(Visibility visibility)
        {
            var popped = CardStack.Pull();
            if (popped is NullCard)
            {
                return null;
            }
            return new Take(this, (Card)popped, visibility, false);
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
            this.Gather(_cardsShuffler.Shuffle(takes.Select(t => t.Card)));
        }

        public override string ToString()
        {
            return $"{Id}: {CardStack}";
        }

        /// <summary>
        /// Player get scored.
        /// arbitrary rule is: Score 3 points when for a won game; Score 1 for a draw.
        /// </summary>
        /// <param name="score"></param>
        public void Scores(int score)
        {
            if (score != 3 && score != 1)
            {
                throw new ArgumentException("Can only score 3 points when for a won game; score 1 for a draw.");
            }

            Score.Increment(score);
        }

        public bool HasCards()
        {
            return CardStack.Any();
        }
    }

}