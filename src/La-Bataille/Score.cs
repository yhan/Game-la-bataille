using System;

namespace LaBataille
{
    /// <summary>
    /// Score of a <see cref="T:LaBataille.Player" />
    /// </summary>
    public class Score : IComparable<Score>, IEquatable<Score>
    {
        private int _value;

        public Score(int value)
        {
            _value = value;
        }

        public void Increment(int points)
        {
            if (points <= 0)
            {
                throw new ArgumentException("Score can only increment");
            }
            _value += points;
        }

        public static Score OfValue(int value)
        {
            return new Score(value);
        }

        public int CompareTo(Score other)
        {
            return _value.CompareTo(other._value);
        }

        public bool Equals(Score other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Score other && Equals(other);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _value;
        }
        
        public override string ToString()
        {
            return this._value.ToString();
        }
    }
}