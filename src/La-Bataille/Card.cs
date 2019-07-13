using System;
using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public struct Card : IEquatable<Card>, IComparable<Card>, IAmCard
    {
        /// <summary>
        /// 2, 3, ..., 10, 11(valet,), 12(dame), 13(roi), 14(as)
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// clubs/trefles (♣), diamonds/carreaux (♦), hearts/coeurs (♥) and spades/pique (♠)
        /// </summary>
        public Figure Figure { get; }

        public static IEnumerable<int> ValidValuesRange = Enumerable.Range(start: 2, count: 13);

        public Card(int value, Figure figure)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException($"Invalid card value {value}");
            }

            Value = value;
            Figure = figure;
        }

        

        public override string ToString()
        {
            return $"{Value}-{Figure}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card card)
            {
                return Value.Equals(card.Value);
            }

            return false;
        }

        public static bool operator ==(Card c1, Card c2)
        {
            return c1.Value == c2.Value;
        }

        public static bool operator !=(Card c1, Card c2)
        {
            return !(c1 == c2);
        }

        public static bool operator >(Card c1, Card c2)
        {
            return c1.Value > c2.Value;
        }

        public static bool operator <(Card c1, Card c2)
        {
            return c1.Value < c2.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        bool IEquatable<Card>.Equals(Card other)
        {
            return Equals(other);
        }

        int IComparable<Card>.CompareTo(Card other)
        {
            return CompareTo(other);
        }

        public bool Equals(Card other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Card other)
        {
            var valueCompare = Value.CompareTo(other.Value);
            if (valueCompare != 0)
            {
                return valueCompare;
            }

            return this.Figure.CompareTo(other.Figure);

        }
    }
}