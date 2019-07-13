using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace La_Bataille
{
    public class Player : ValueType<Player>
    {
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
        }

        public CardStack CardStack { get; set; }

        public Take Lever(Visibility visibility)
        {
            var popped = CardStack.Pull();
            if (popped is NullCard)
            {
                return null;
            }
            return new Take(this, (Card)popped, visibility);
        }

        /// <summary>
        /// Player owns new <param name="cards"></param>
        /// </summary>
        /// <param name="cards"></param>
        public void Gather(IEnumerable<Card> cards)
        {
            foreach (var carte in cards)
            {
                CardStack.Carpet(carte);
            }
        }

        public void Gather(IEnumerable<Take> takes)
        {
            this.Gather(takes.Select(x => x.Card)
                .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);
        }

        
        

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Id };
        }

        public override string ToString()
        {
            return $"{Id}: {CardStack}";
        }

        public void Scores(int score)
        {
            throw new System.NotImplementedException();
        }
    }


    public class Competition
    {
        private readonly Game[] _games;

        public Competition(Game[] games)
        {
            _games = games;
        }

        Dictionary<Player, Score> _rankingTable = new Dictionary<Player, Score>();

        public Ranking Play()
        {
            //foreach (var game in _games)
            //{
            //    var gameOver = game.Play(NullShuffle.Instance);
            //    switch (gameOver)
            //    {
            //        case Draw _:
            //            foreach (var player in game.Players)
            //            {
            //                player.Scores(1);
            //            }

            //        break;

            //        case HasWinner hasWinner:
            //            var winner = hasWinner.Winner;
            //            winner.Scores(3);

            //        break;
            //    }
            //}

            throw new NotImplementedException();
        }
    }

    public class Score
    {
    }

    public class Ranking : IEnumerable<Rank>
    {
        private readonly List<Rank> _ranks = new List<Rank>();

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
        public int NumberOfWonGames { get; }

        public Rank(int number, Player player, int numberOfWonGames)
        {
            Number = number;
            Player = player;
            NumberOfWonGames = numberOfWonGames;
        }
    }
}