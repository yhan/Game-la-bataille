using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace LaBataille
{
    public class Player : ValueType<Player>
    {
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
        }

        public CardStack CardStack { get; set; }
        public Score Score { get; } = new Score(0);

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
            Score.Increment(score);
        }
    }


    public class GameFactory: IMakeGames
    {
        private readonly IDistributeCards _cardsDistributor;

        public GameFactory( IDistributeCards cardsDistributor)
        {
            _cardsDistributor = cardsDistributor;
        }

        public Game Build()
        {
            return new Game(_cardsDistributor);
        }
    }

   
    public interface IMakeGames
    {
        Game Build();
    }


    public class Competition
    {
        private readonly List<Player> _players;
        private readonly List<Game> _games = new List<Game>();

        public Competition(int numberOfRounds, IMakeGames factory, List<Player> players)
        {
            _players = players;
            
            for (int round = 0; round < numberOfRounds; round++)
            {
                _games.Add(factory.Build());
            }
        }
        
        public Ranking Play()
        {
            foreach (var game in _games)
            {
                var gameOver = game.Play(NullShuffle.Instance);
                switch (gameOver)
                {
                    case Draw _:
                        foreach (var player in game.Players)
                        {
                            player.Scores(1);
                        }

                        break;

                    case HasWinner hasWinner:
                        var winner = hasWinner.Winner;
                        winner.Scores(3);

                        break;
                }
            }
            
            return new Ranking(_players);
        }
    }

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
    }

    public class Ranking : IEnumerable<Rank>
    {

        public Ranking(IEnumerable<Player> playersOrderedByScore)
        {
            Ranks = playersOrderedByScore.OrderByDescending(p => p.Score).Select((p, i) => new Rank(i + 1, p));
        }

        public IEnumerable<Rank> Ranks;


        public IEnumerator<Rank> GetEnumerator()
        {
            return Ranks.GetEnumerator();
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

        public Rank(int number, Player player)
        {
            Number = number;
            Player = player;
        }
    }
}