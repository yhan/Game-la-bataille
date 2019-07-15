using System;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace LaBataille.Tests
{
    [TestFixture]
    public class CardsProviderShould
    {
        [Test]
        public void Provide_all_52_card()
        {
            var provider = new CardsProvider();
            var cards = provider.Provide().ToArray();

            Console.WriteLine(string.Join(", ", cards));

            var everyCardValueHasCards = 4;

            foreach (var cardValue in Card.ValidValuesRange)
            {
                Check.That(cards.Where(c => c.Value == cardValue)).HasSize(everyCardValueHasCards);
                Check.That(cards.Select(c => c.Figure).Distinct()).IsEquivalentTo(Figure.Club, Figure.Diamond, Figure.Heart, Figure.Spade);
            }
        }
    }
}