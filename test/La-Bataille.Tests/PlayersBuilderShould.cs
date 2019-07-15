using System;
using NFluent;
using NUnit.Framework;

namespace LaBataille.Tests
{
    [TestFixture]
    public class PlayersBuilderShould
    {
        [Test]
        public void Should_have_at_least_2_players_in_a_game()
        {
            Check.ThatCode(() => PlayersBuilder.BuildPlayers(1, new CardsShufflerForTest())).Throws<ArgumentException>()
                .WithMessage("Should have at least 2 players in a game");
        }
    }
}