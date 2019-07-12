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
            var trefles10 = 10.AsClub();
            var pique10 = 10.AsSpade();
            var coeur10 = 10.AsHeart();
            var carreaux10 = 10.AsDiamond();

            Check.That(trefles10).IsEqualTo(pique10);
            Check.That(pique10).IsEqualTo(coeur10);
            Check.That(coeur10).IsEqualTo(carreaux10);

            Check.That(trefles10 == pique10).IsEqualTo(true);
            Check.That(pique10 == coeur10).IsEqualTo(true);
            Check.That(coeur10 == carreaux10).IsEqualTo(true);
        }

        [Test]
        public void Can_compare_cards_When_not_equals()
        {
            var trefles10 = 10.AsClub();
            var pique11 = 11.AsSpade();

            Check.That(pique11 > trefles10).IsTrue();
        }
    }
}