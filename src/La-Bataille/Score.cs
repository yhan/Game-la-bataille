using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    public class Score : IComparable<Score>, IEquatable<Score>
    {
        private int _value;

        public Score(int value)
        {
            _value = value;
        }

        public void Increment(int points)
        {
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
            return _value;
        }
        
        public override string ToString()
        {
            return this._value.ToString();
        }
    }

    public class Ranking : IEnumerable<Rank>
    {
        private readonly IEnumerable<Rank> _ranks;

        public Ranking(IEnumerable<Player> playersOrderedByScore)
        {
            _ranks = playersOrderedByScore.OrderByDescending(p => p.Score).Select((p, i) => new Rank(i + 1, p));
        }


        public IEnumerator<Rank> GetEnumerator()
        {
            return _ranks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class Rank
    {
        public int Number { get; }
        public Player Player { get; }

        public Score Score => Player.Score;

        public Rank(int number, Player player)
        {
            Number = number;
            Player = player;
        }
    }
}