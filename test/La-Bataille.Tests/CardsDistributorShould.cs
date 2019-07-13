using System;
using System.Collections.Generic;
using System.Linq;
using La_Bataille;
using NFluent;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CardsDistributorShould
    {
        [Test]
        public void Can_distribute_provided_cards_to_equal_number_of_parts()
        {
            var numberOfPlayers = 4;
            var expectedCardsStackSize = 13;

            var distributor = new CardsDistributor(new CardsProvider(), numberOfPlayers);
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
        public void Can_distribute_provided_cards_Without_missing_cards_And_no_redundent_cards()
        {
            var numberOfPlayers = 4;
            
            var distributor = new CardsDistributor(new CardsProvider(), numberOfPlayers);
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

            var distributor = new CardsDistributor(new CardsProvider(), numberOfPlayers);
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
    }
}