using System.Linq;
using La_Bataille;
using NFluent;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CompetitionShould : GameShould
    {
        [Test]
        public void Can_play_n_rounds_and_rank_players()
        {
            var players = Enumerable.Range(0, count: 2).Select(x => new Player(x)).ToList();

            var distributor = new DistributorForTest(players);

            var competition = new Competition(new TestGamesProvider(players, 3, distributor));

            Ranking ranking = competition.Play();
            Check.That(ranking).HasSize(2);

            var player1 = players[0];
            var player2 = players[1];
            Check.That(ranking.OrderBy(r => r.Number).Select(r => r.Player)).ContainsExactly(player1, player2);
            Check.That(player1.Score).IsEqualTo(Score.OfValue(6));
            Check.That(player2.Score).IsEqualTo(Score.OfValue(3));
        }
    }
}