using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace La_Bataille
{
    /// <summary>
    /// Play take a card makes it a `Take`
    /// </summary>
    public class Take : IComparable<Take>
    {
        public Player Player { get; }
        public Card Card { get; }
        public Visibility Visibility { get; }

        public Take(Player player, Card card, Visibility visibility)
        {
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
    }


    public static class TakeExtensions
    {
        public static View BuildView(this IEnumerable<Take> takes)
        {
            return new View(takes.Select(take => new TwoFaceCard(/*take.Player,*/ take.Card, take.Visibility)).ToArray());
        }

        public static List<Take> KeepTheLast(this IEnumerable<Take> takes, int numberOfPlayersInTheGame)
        {
            return takes.Reverse().Take(numberOfPlayersInTheGame).ToList();
        }
    }
}