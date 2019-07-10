using System;
using System.Collections.Generic;
using System.Linq;
using La_Bataille;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    public class CardShould
    {
        [Test]
        public void Can_compare_cards_When_equality()
        {
            var clubs10 = new Carte(10, "trefle");
            var diamonds10 = new Carte(10, "pique");
            var hearts10 = new Carte(10, "coeur");
            var spades10 = new Carte(10, "carreau");

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
            var clubs10 = new Carte(10, "trefle");
            var spadesJack = new Carte(11,  "pique");

            Check.That(spadesJack > clubs10).IsTrue();
        }
    }


    [TestFixture]
    public class BatailleShould
    {
        [Test]
        public void Can_play_la_premiere_levee()
        {
            var clubs10 = new Carte(10, "trefle");
            var spadesJack = new Carte(11,  "pique");
            var shuffle = NSubstitute.Substitute.For<IShuffle>();
            var bataille = new Bataille(2, shuffle);
            shuffle.DistributeCartes(bataille.Joueurs).Returns(new List<Joueur>
            {
                new Joueur(0){Cartes = new Stack<Carte>(new []{clubs10})},
                new Joueur(1) {Cartes = new Stack<Carte>(new[] {spadesJack})}
            });


            bataille.Start();

            Check.That(bataille.VuesPlateau).HasSize(1);
            Check.That(bataille.VuesPlateau[0].Value).Contains(clubs10, spadesJack);
        }

        [Test]
        public void stacktest()
        {
            var stack = new Stack<int>(new[] {1, 2, 3});
            Gagner(stack, new[] {4, 5});
        }

        private void Gagner(Stack<int> stack, int[] bottom)
        {
            new Stack<int>(bottom.Contains(stack))
        }
    }
}