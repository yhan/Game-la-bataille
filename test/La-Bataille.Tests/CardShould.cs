﻿using System;
using System.Linq;
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
            var spadesJack = new Carte(11, "pique");

            Check.That(spadesJack > clubs10).IsTrue();
        }
    }
}