using System;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    /// Play take a card makes it a `Take`
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
        
        public static Player StrongestPlayerIfExit(this IEnumerable<Take> takes)
        {
            var array = takes as Take[] ?? takes.ToArray();
            var take = array.Max();
            if (array.Count(t => t.CompareTo(take) == 0) == 1)
            {
                return take.Player;
            }

            return null;
        }

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

        public static void Drop(this IEnumerable<Take> takes)
        {
            foreach (var take in takes)
            {
                take.Drop();
            }
        }
    }
}