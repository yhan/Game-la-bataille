using System;

namespace La_Bataille
{
    public struct Card: IEquatable<Card>, IComparable<Card>
    {
        public int Value { get; }
        
        /// <summary>
        /// clubs (♣), diamonds (♦), hearts (♥) and spades (♠)
        /// </summary>
        public string Color { get; }

        public Card(int value, string color)
        {
            Value = value;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            if(obj is Card card)
            {
                return Value.Equals(card.Value);
            }

            return false;
        }

        public static bool operator == (Card c1, Card c2)
        {
            return c1.Value == c2.Value;
        }

        public static bool operator !=(Card c1, Card c2)
        {
            return !(c1 == c2);
        }

        public static bool operator > (Card c1, Card c2)
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
            return Value.CompareTo(other.Value);
        }
    }
}