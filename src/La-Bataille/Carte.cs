﻿using System;

namespace La_Bataille
{
    public struct Carte : IEquatable<Carte>, IComparable<Carte>, IAmCarte
    {
        /// <summary>
        /// 2, 3, ..., 10, 11(valet,), 12(dame), 13(roi), 14(as)
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// clubs/trefles (♣), diamonds/carreaux (♦), hearts/coeurs (♥) and spades/pique (♠)
        /// </summary>
        public Figure Figure { get; }

        public Carte(int value, Figure figure)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException($"Invalid carte value {value}");
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
            if (obj is Carte card)
            {
                return Value.Equals(card.Value);
            }

            return false;
        }

        public static bool operator ==(Carte c1, Carte c2)
        {
            return c1.Value == c2.Value;
        }

        public static bool operator !=(Carte c1, Carte c2)
        {
            return !(c1 == c2);
        }

        public static bool operator >(Carte c1, Carte c2)
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
            var valueCompare = Value.CompareTo(other.Value);
            if (valueCompare != 0)
            {
                return valueCompare;
            }

            return this.Figure.CompareTo(other.Figure);

        }
    }
}