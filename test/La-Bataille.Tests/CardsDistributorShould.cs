using System;
using System.Collections.Generic;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace LaBataille.Tests
{
    [TestFixture]
    public class CardsDistributorShould
    {
        [Test]
        public void Can_distribute_provided_cards_to_equal_number_of_parts()
        {
            var numberOfPlayers = 4;
            var expectedCardsStackSize = 13;

            var distributor = new CardsDistributor(new CardsProvider(), PlayersBuilder.BuildPlayers(numberOfPlayers, new CardsShufflerForTest()));
            var players = distributor.Distribute();

            for (int id = 0; id < numberOfPlayers; id++)
            {
                var player = players[id];
                Check.That(player.Id).IsEqualTo(id);
                
                Check.That(player.CardStack).HasSize(expectedCardsStackSize);
            }

            Check.That(distributor.DistributedCardsSize).IsEqualTo(52);
        }


        [Test]
        public void Can_distribute_provided_cards_Without_missing_cards_And_no_redundant_cards()
        {
            var numberOfPlayers = 4;
            
            var distributor = new CardsDistributor(new CardsProvider(), PlayersBuilder.BuildPlayers(numberOfPlayers, new CardsShufflerForTest()));
            var players = distributor.Distribute();

            var allCards = players.Select(p => p.CardStack).SelectMany(c => c).ToArray();
            var allFigures = allCards.GroupBy(c => c.Figure)
                                    .Select(f => f.Key)
                                    .Distinct();

            
            Check.That(allFigures).IsEquivalentTo(Figure.Club, Figure.Diamond, Figure.Heart, Figure.Spade);

            Check.That(allCards.Select(c => c.Value).Distinct()).IsEquivalentTo(Card.ValidValuesRange);


            foreach (List<Card> cardsOfTheSameValue in allCards
                                       .GroupBy(c => c.Value).ToDictionary(c => c.Key, c => c?.ToList()).Values)
            {
                Check.That(cardsOfTheSameValue).HasSize(4);
                Check.That(cardsOfTheSameValue.Select(c => c.Figure)).IsEquivalentTo(Figure.Club, Figure.Diamond, Figure.Heart, Figure.Spade);
            }
        }

        [Test]
        public void Can_distribute_provided_cards_to_equal_number_of_parts_When_cards_are_not_dividable()
        {
            var numberOfPlayers = 3;
            var expectedCardsStackSize = 17;

            var distributor = new CardsDistributor(new CardsProvider(), PlayersBuilder.BuildPlayers(numberOfPlayers, new CardsShufflerForTest()));
            var players = distributor.Distribute();

            for (int id = 0; id < numberOfPlayers; id++)
            {
                var player = players[id];
                Check.That(player.Id).IsEqualTo(id);
                
                Check.That(player.CardStack).HasSize(expectedCardsStackSize);
            }

            Check.That(distributor.DistributedCardsSize).IsEqualTo(51);

            foreach (var player in players)
            {
                Console.WriteLine(player);
                Console.WriteLine("#########################################################");
            }
        }
        
        [Test]
        public void Have_at_least_3_cards_each_for_all_players([Range(18, 52/*Here should be int.Max, but this hangs my computer*/)]int numberOfPlayers)
        {
            Check.ThatCode(() =>
            {
                var distributor = new CardsDistributor(new CardsProvider(), PlayersBuilder.BuildPlayers(numberOfPlayers, new CardsShufflerForTest()));
            }).Throws<ArgumentException>()
                .WithMessage("Each player should have at least 3 cards. Number of players can not exceed 17. ");
        }


        [Test]
        public void Should_have_at_least_2_players_in_a_game()
        {
            Check.ThatCode(() => new CardsDistributor(CardsProvider.Instance, new List<Player> {new Player(0, new CardsShufflerForTest())}))
                .Throws<ArgumentException>()
                .WithMessage("Should have at least 2 players in a game");
        }

        [Test]
        public void Can_shuffle_cards()
        {
            var cardsProvider = new CardsProvider();
            var cards = cardsProvider.Provide().ToList();

            var shuffled = cards.ShuffleList().ToList();

            Check.That(shuffled.Count).IsEqualTo(cards.Count);
            Check.That(shuffled).IsEquivalentTo(cards);
            Check.ThatCode(() => { Check.That(shuffled).ContainsExactly(cards); }).Throws<NUnit.Framework.AssertionException>();
        }


        [Test]
        public void Can_shuffle_cards2()
        {
            var cardsProvider = new CardsProvider();
            var cards = cardsProvider.Provide().ToList();

            var original = new List<Card>(cards);
            
            var shuffled = cards.Shuffle();

            Check.That(shuffled.Count).IsEqualTo(cards.Count);
            Check.That(shuffled).IsEquivalentTo(cards);
            Check.ThatCode(() => { Check.That(shuffled).ContainsExactly(original); }).Throws<NUnit.Framework.AssertionException>();
        }
    }
}