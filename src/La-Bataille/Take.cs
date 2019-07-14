using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return (Card.Value).CompareTo(other.Card.Value);
        }

        public override string ToString()
        {
            return $"{Player.Id}: {Card}";
        }

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
            var first = takes.First();
            foreach (var take in takes)
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