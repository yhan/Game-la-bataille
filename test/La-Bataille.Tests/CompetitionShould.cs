using System.Linq;
using NFluent;
using NUnit.Framework;

namespace LaBataille.Tests
{
    [TestFixture]
    public class CompetitionShould 
    {
        [Test]
        public void Can_play_n_rounds_and_rank_players()
        {
            var players = Enumerable.Range(0, count: 2).Select(x => new Player(x)).ToList();

            var distributor = new DistributorForTest(players);

            var competition = new Competition(numberOfRounds: 3, factory: new GameFactory(distributor), players: players);

            Ranking ranking = competition.Play(NullGameOverVisitor.Instance);
            Check.That(ranking).HasSize(2);

            var player1 = players[0];
            var player2 = players[1];
            Check.That(ranking.OrderBy(r => r.Number).Select(r => r.Player)).ContainsExactly(player1, player2);
            Check.That(player1.Score).IsEqualTo(Score.OfValue(6));
            Check.That(player2.Score).IsEqualTo(Score.OfValue(3));
        }


        [Test]
        public void Can_play_n_rounds_and_rank_players_with_draw_game()
        {
            var players = Enumerable.Range(0, count: 3).Select(x => new Player(x)).ToList();

            var distributor = new DistributorWithDrawForTest(players);

            var competition = new Competition(2, new GameFactory(cardsDistributor: distributor), players);

            Ranking ranking = competition.Play(NullGameOverVisitor.Instance);
            Check.That(ranking).HasSize(3);

            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];
            Check.That(ranking.OrderBy(r => r.Number).Select(r => r.Player)).ContainsExactly(player3, player1, player2);
            Check.That(player1.Score).IsEqualTo(Score.OfValue(1));
            Check.That(player2.Score).IsEqualTo(Score.OfValue(1));
            Check.That(player3.Score).IsEqualTo(Score.OfValue(4));
        }

               #region cards

        protected readonly Card d2 = 2.AsDiamond();
        protected readonly Card d3 = 3.AsDiamond();
        protected readonly Card d4 = 4.AsDiamond();
        protected readonly Card d5 = 5.AsDiamond();
        protected readonly Card d6 = 6.AsDiamond();
        protected readonly Card d7 = 7.AsDiamond();
        protected readonly Card d8 = 8.AsDiamond();
        protected readonly Card d10 = 10.AsDiamond();
        protected readonly Card d11 = 11.AsDiamond();
        protected readonly Card d12 = 12.AsDiamond();
        protected readonly Card d14 = 14.AsDiamond();

        protected readonly Card s2 = 2.AsSpade();
        protected readonly Card s3 = 3.AsSpade();
        protected readonly Card s5 = 5.AsSpade();
        protected readonly Card s6 = 6.AsSpade();
        protected readonly Card s7 = 7.AsSpade();
        protected readonly Card s8 = 8.AsSpade();
        protected readonly Card s11 = 11.AsSpade();
        protected readonly Card s12 = 12.AsSpade();
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
        protected readonly Card h3 = 3.AsHeart();
        protected readonly Card h4 = 4.AsHeart();
        protected readonly Card h5 = 5.AsHeart();
        protected readonly Card h6 = 6.AsHeart();
        protected readonly Card h7 = 7.AsHeart();
        protected readonly Card h8 = 8.AsHeart();
        protected readonly Card h9 = 9.AsHeart();
        protected readonly Card h10 = 10.AsHeart();
        protected readonly Card h11 = 11.AsHeart();
        protected readonly Card h12 = 12.AsHeart();
        protected readonly Card h14 = 14.AsHeart();

        #endregion
    }


    internal class  NullGameOverVisitor : IVisitGameOver
    {
        public static IVisitGameOver Instance = new NullGameOverVisitor();

        private NullGameOverVisitor(){ }
        
        public void Visit(Game game, IAmTheGameOver gameOver)
        {
        }
    }
}