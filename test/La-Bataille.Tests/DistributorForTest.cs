using System.Collections.Generic;

namespace LaBataille.Tests
{
    public class DistributorWithDrawForTest : IDistributeCards
    {
        private readonly List<Player> _players;
        private int _count;


        public DistributorWithDrawForTest(List<Player> players)
        {
            _players = players;
        }

        public List<Player> Distribute()
        {
            List<Player> distribution;

            if (_count == 0)
            {
                distribution = _players.With(new List<List<Card>>
                {
                    //draw
                    new List<Card>{d14, d8, d7},
                    new List<Card>{s14, s8, s7},
                    new List<Card>{c14, c8, c7}
                });

                DistributedCardsSize = 9;
            }
            else
            {
                distribution = _players.With(new List<List<Card>>
                {
                    // winner player 2
                    new List<Card> {s2, d3, d4},
                    new List<Card> {s3, d2, c5},
                    new List<Card> {s13, s14, c2}
                });

                DistributedCardsSize = 9;
            }

            _count++;
            return distribution;
        }

        public int DistributedCardsSize { get; set; }


        #region cards

        protected readonly Card d2 = 2.AsDiamond();
        protected readonly Card d3 = 3.AsDiamond();
        protected readonly Card d4 = 4.AsDiamond();
        protected readonly Card d7 = 7.AsDiamond();
        protected readonly Card d8 = 8.AsDiamond();
        protected readonly Card d11 = 11.AsDiamond();
        protected readonly Card d14 = 14.AsDiamond();

        protected readonly Card s2 = 2.AsSpade();
        protected readonly Card s3 = 3.AsSpade();
        protected readonly Card s5 = 5.AsSpade();
        protected readonly Card s7 = 7.AsSpade();
        protected readonly Card s8 = 8.AsSpade();
        protected readonly Card s11 = 11.AsSpade();
        protected readonly Card s13 = 13.AsSpade();
        protected readonly Card s14 = 14.AsSpade();

        protected readonly Card c2 = 2.AsClub();
        protected readonly Card c4 = 4.AsClub();
        protected readonly Card c5 = 5.AsClub();
        protected readonly Card c6 = 6.AsClub();
        protected readonly Card c7 = 7.AsClub();
        protected readonly Card c8 = 8.AsClub();
        protected readonly Card c9 = 9.AsClub();
        protected readonly Card c10 = 10.AsClub();
        protected readonly Card c11 = 11.AsClub();
        protected readonly Card c14 = 14.AsClub();


        protected readonly Card h2 = 2.AsHeart();
        protected readonly Card h5 = 5.AsHeart();
        protected readonly Card h7 = 7.AsHeart();
        protected readonly Card h8 = 8.AsHeart();
        protected readonly Card h9 = 9.AsHeart();
        protected readonly Card h11 = 11.AsHeart();
        protected readonly Card h14 = 14.AsHeart();

        #endregion
    }



    public class DistributorForTest : IDistributeCards
    {
        private readonly List<Player> _players;
        private int _count;

        public DistributorForTest(List<Player> players)
        {
            _players = players;
        }

        public List<Player> Distribute()
        {
            List<Player> distribution;

            if (_count == 0)
            {
                distribution = _players.With(new List<List<Card>>
                {
                    //#4 -winner player 0
                    new List<Card> {d8, s3, d2, c6}, new List<Card> {c7, d4, s2, c5}
                });

                DistributedCardsSize = 8;
            }
            else if (_count == 1)
            {
                distribution = _players.With(new List<List<Card>>
                {
                    //#3 -winner player 1
                    new List<Card> {s2, d3, c4},
                    new List<Card> {s3, d2, c5}
                });
                DistributedCardsSize = 6;
            }
            else
            {
                distribution = _players.With(new List<List<Card>>
                {
                    //#1 - winner player 0
                    new List<Card> {s14, d8, s5, s3, d2, c6},
                    new List<Card> {s11, c9, h5, d4, s2, c5}
                });
                DistributedCardsSize = 12;
            }

            _count++;
            return distribution;
        }


        #region cards

        protected readonly Card d2 = 2.AsDiamond();
        protected readonly Card d3 = 3.AsDiamond();
        protected readonly Card d4 = 4.AsDiamond();
        protected readonly Card d7 = 7.AsDiamond();
        protected readonly Card d8 = 8.AsDiamond();
        protected readonly Card d11 = 11.AsDiamond();
        protected readonly Card d14 = 14.AsDiamond();

        protected readonly Card s2 = 2.AsSpade();
        protected readonly Card s3 = 3.AsSpade();
        protected readonly Card s5 = 5.AsSpade();
        protected readonly Card s7 = 7.AsSpade();
        protected readonly Card s8 = 8.AsSpade();
        protected readonly Card s11 = 11.AsSpade();
        protected readonly Card s13 = 13.AsSpade();
        protected readonly Card s14 = 14.AsSpade();

        protected readonly Card c2 = 2.AsClub();
        protected readonly Card c4 = 4.AsClub();
        protected readonly Card c5 = 5.AsClub();
        protected readonly Card c6 = 6.AsClub();
        protected readonly Card c7 = 7.AsClub();
        protected readonly Card c8 = 8.AsClub();
        protected readonly Card c9 = 9.AsClub();
        protected readonly Card c10 = 10.AsClub();
        protected readonly Card c11 = 11.AsClub();
        protected readonly Card c14 = 14.AsClub();


        protected readonly Card h2 = 2.AsHeart();
        protected readonly Card h5 = 5.AsHeart();
        protected readonly Card h7 = 7.AsHeart();
        protected readonly Card h8 = 8.AsHeart();
        protected readonly Card h9 = 9.AsHeart();
        protected readonly Card h11 = 11.AsHeart();
        protected readonly Card h14 = 14.AsHeart();

        #endregion

        public int DistributedCardsSize { get; set; }
    }
}
