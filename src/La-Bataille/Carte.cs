using System;

namespace La_Bataille
{
    public struct Carte: IEquatable<Carte>, IComparable<Carte>
    {
        public uint Value { get; }
        
        /// <summary>
        /// clubs (♣), diamonds (♦), hearts (♥) and spades (♠)
        /// </summary>
        public string Color { get; }

        public Carte(uint value, string color)
        {
            // 2, 3, ..., 10, 11(j), 12(q), 13(k), 14(A)

            if(value < 2 || value > 14)
            {
                throw new ArgumentException($"Invalid carte value {value}");
            }

            Value = value;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            if(obj is Carte card)
            {
                return Value.Equals(card.Value);
            }

            return false;
        }

        public static bool operator == (Carte c1, Carte c2)
        {
            return c1.Value == c2.Value;
        }

        public static bool operator !=(Carte c1, Carte c2)
        {
            return !(c1 == c2);
        }

        public static bool operator > (Carte c1, Carte c2)
        {
            return c1.Value > c2.Value;
        }

        public static bool operator <(Carte c1, Carte c2)
        {
            return c1.Value < c2.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        bool IEquatable<Carte>.Equals(Carte other)
        {
            return Equals(other);
        }

        int IComparable<Carte>.CompareTo(Carte other)
        {
            return CompareTo(other);
        }

        public bool Equals(Carte other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Carte other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}