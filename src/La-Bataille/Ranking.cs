using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    /// A ranking table composed of the <see cref="Rank"/> of all players.
    /// </summary>
    public class Ranking : IEnumerable<Rank>
    {
        private readonly IEnumerable<Rank> _ranks;

        public Ranking(IEnumerable<Player> players)
        {
            _ranks = players.OrderByDescending(p => p.Score).Select((p, i) => new Rank(i + 1, p));
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

    /// <summary>
    /// The rank of a <see cref="Player"/> 
    /// </summary>
    public class Rank
    {
        /// <summary>
        /// The position of Player in the ranking
        /// </summary>
        public int Number { get; }

        public Player Player { get; }

        /// <summary>
        /// The score of <see cref="Player"/>
        /// </summary>
        public Score Score => Player.Score;

        public Rank(int number, Player player)
        {
            Number = number;
            Player = player;
        }
    }

}