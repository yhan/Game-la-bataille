using La_Bataille;
using NFluent;
using NUnit.Framework;

namespace Tests
{
    public class CardShould
    {
        [Test]
        public void Can_compare_cards_When_equality()
        {
            var clubs10 = new Card(10, "clubs");
            var diamonds10 = new Card(10, "diamonds");
            var hearts10 = new Card(10, "hearts");
            var spades10 = new Card(10, "spades");

            Check.That(clubs10).IsEqualTo(diamonds10);
            Check.That(diamonds10).IsEqualTo(hearts10);
            Check.That(hearts10).IsEqualTo(spades10);

            Check.That(clubs10 == diamonds10).IsEqualTo(true);
            Check.That(diamonds10 == hearts10).IsEqualTo(true);
            Check.That(hearts10 == spades10).IsEqualTo(true);
        }

        [Test]
        public void Can_compare_cards_When_not_equals()
        {
            var clubs10 = new Card(10, "clubs");
            var spadesJack = new Card(11,  "spades");

            Check.That(spadesJack > clubs10).IsTrue();
        }
    }
}