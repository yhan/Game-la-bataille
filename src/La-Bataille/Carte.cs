using System;

namespace La_Bataille
{

    public static class CarteExtensions
    {
        public static Carte AsTrefles(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Trefle);
        }

        public static Carte AsCarreaux(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Carreaux);
        }


        public static Carte AsCoeur(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Coeur);
        }


        public static Carte AsPique(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Pique);
        }


    }


    public enum Figure
    {
        Carreaux = 0,
        Coeur,
        Pique,
        Trefle
    }

    public struct Carte: IEquatable<Carte>, IComparable<Carte>
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
            if(value < 2 || value > 14)
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