using System.Collections.Generic;
using La_Bataille;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BatailleShould
    {
        [Test]
        public void Can_play_la_premiere_levee()
        {
            var dixTrefle = new Carte(10, "trefle");
            var piqueValet = new Carte(11, "pique");
            var shuffle = Substitute.For<IShuffle>();
            var bataille = new Bataille(2, shuffle);
            shuffle.DistributeCartes(bataille.Joueurs).Returns(new List<Joueur>
            {
                new Joueur(0){Cartes = new List<Carte>(new []{dixTrefle})},
                new Joueur(1) {Cartes = new List<Carte>(new []{piqueValet})}
            });

            bataille.Start();

            Check.That(bataille.VuesPlateau).HasSize(1);
            Check.That(bataille.VuesPlateau[0].Cartes).Contains(dixTrefle, piqueValet);

            Check.That(bataille.Joueurs[0].Cartes).HasSize(0);
            Check.That(bataille.Joueurs[0].Cartes).IsOnlyMadeOf(dixTrefle, piqueValet);
        }
    }
}