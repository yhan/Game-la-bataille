using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
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