using System;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    /// Player takes a card, is called a `Take`
    /// </summary>
    public class Take : IComparable<Take>
    {
        public Player Player { get; }
        public Card Card { get; }
        public Visibility Visibility { get; }

        public bool Dropped { get; private set; }

        public Take(Player player, Card card, Visibility visibility, bool dropped)
        {
            Dropped = dropped;
            Player = player;
            Card = card;
            Visibility = visibility;
        }

        public int CompareTo(Take other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Card.Value.CompareTo(other.Card.Value);
        }

        public override string ToString()
        {
            return $"{Player.Id}: {Card}";
        }

        /// <summary>
        /// In some situations, as we are unable to determine who should gather the card, we drop it.
        /// (leave it out of the game, nobody owns it.)
        /// </summary>
        public void Drop()
        {
            if (Dropped)
            {
                throw new InvalidOperationException($"{this} is already dropped");
            }

            Dropped = true;
        }
    }


    public static class TakeExtensions
    {
        /// <summary>
        /// After all players have took a card, build a <see cref="View"/> of table.
        /// </summary>
        public static View BuildView(this IEnumerable<Take> takes)
        {
            return new View(takes.Select(take => new TwoFaceCard(take.Player, take.Card, take.Visibility)).ToArray());
        }

        /// <summary>
        /// Keep the last <param name="numberOfPlayersInTheGame"></param> takes 
        /// </summary>
        public static List<Take> KeepTheLast(this IEnumerable<Take> takes, int numberOfPlayersInTheGame)
        {
            return takes.Reverse().Take(numberOfPlayersInTheGame).ToList();
        }
        
        /// <summary>
        /// Try to find the <see cref="Player"/> who has the strongest card, and only he has that card
        /// </summary>
        public static Player UniqueStrongestPlayerIfExit(this IEnumerable<Take> takes)
        {
            var array = takes as Take[] ?? takes.ToArray();
            var take = array.Max();
            if (array.Count(t => t.CompareTo(take) == 0) == 1)
            {
                return take.Player;
            }

            return null;
        }

        /// <summary>
        /// Check if all takes have exactly the same value of <see cref="Card"/>
        /// </summary>
        /// <param name="takes"></param>
        /// <returns></returns>
        public static bool AllEqual(this IEnumerable<Take> takes)
        {
            var takesArray = takes as Take[] ?? takes.ToArray();
            var first = takesArray.First();
            foreach (var take in takesArray)
            {
                if (take.CompareTo(first) != 0) return false;
            }

            return true;
        }

        /// <summary>
        /// Drop all <param name="takes"></param>
        /// </summary>
        /// <param name="takes"></param>
        public static void Drop(this IEnumerable<Take> takes)
        {
            foreach (var take in takes)
            {
                take.Drop();
            }
        }
    }
}