using System.Collections.Generic;
using System.Linq;
using La_Bataille;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BatailleShould
    {
        private readonly Carte t5 = 5.AsTrefles();
        private readonly Carte t4 = 4.AsTrefles();
        private readonly Carte p2 = 2.AsPique();
        private readonly Carte c3 = 3.AsCarreaux();
        private readonly Carte c2 = 2.AsCarreaux();
        private readonly Carte p3 = 3.AsPique();
        private readonly Carte c4 = 4.AsCarreaux();
        private readonly Carte p13 = 13.AsPique();
        private readonly Carte p14 = 14.AsPique();
        private readonly Carte t2 = 2.AsTrefles();

        [Test]
        public void Can_play_la_premiere_levee()
        { 
            var dixTrefle = 10.AsTrefles();
            var piqueValet = 11.AsPique();
            var bataille = BuildGame(2, new List<List<Carte>>
            {
                new List<Carte> {dixTrefle},
                new List<Carte> {piqueValet}
            });

            bataille.Start(NullShuffle.Instance);

            Check.That(bataille.VuesPlateau).HasSize(1);
            Check.That(bataille.VuesPlateau[0].AsPureCartes()).Contains(dixTrefle, piqueValet);

            Check.That(bataille.Joueurs[0].Paquet).HasSize(0);

            Check.That(bataille.Joueurs[1].Paquet).HasSize(2);
            Check.That(bataille.Joueurs[1].Paquet).Contains(dixTrefle, piqueValet);


            bool gameOver = bataille.JeuEnded(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(bataille.Joueurs[1]);
        }

        [Test]
        public void Can_play_n_levees_to_the_end_of_game()
        {
            var bataille = BuildGame(2, new List<List<Carte>>
            {
                new List<Carte> {p2, c3, t4},
                new List<Carte> {p3, c2, t5}
            });

            bataille.Start(NullShuffle.Instance);

            Check.That(bataille.VuesPlateau).HasSize(5);
            Check.That(bataille.VuesPlateau[0].AsPureCartes()).Contains(t5, t4);
            Check.That(bataille.VuesPlateau[1].AsPureCartes()).Contains(c2, c3);
            Check.That(bataille.VuesPlateau[2].AsPureCartes()).Contains(p3, p2);
            Check.That(bataille.VuesPlateau[3].AsPureCartes()).Contains(t5, c3);
            Check.That(bataille.VuesPlateau[4].AsPureCartes()).Contains(c2, t4);


            Check.That(bataille.Joueurs[0].Paquet).HasSize(0);
            Check.That(bataille.Joueurs[1].Paquet).HasSize(6);
            Check.That(bataille.Joueurs[1].Paquet).Contains(t5, t4, p2, c3, c2, p3);
            
            bool gameOver = bataille.JeuEnded(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(bataille.Joueurs[1]);
        }



        [Test]
        public void Can_find_gagnant_When_bataille_happened()
        {
            var bataille = BuildGame(numberOfJoueurs: 3, distribution: new List<List<Carte>>
            {
                new List<Carte> {p2, c3, c4},
                new List<Carte> {p3, c2, t5},
                new List<Carte> {p13, p14, t2}
            });

            var shuffle = Substitute.For<IShuffle>();

            bataille.Start(shuffle);

            Check.That(bataille.VuesPlateau).HasSize(8);
            Check.That(bataille.VuesPlateau[0]).Contains(c4, t5, t2);
            Check.That(bataille.VuesPlateau[1]).Contains(c2, c3, p14);
            Check.That(bataille.VuesPlateau[2]).Contains(p3, p2, p13);
            Check.That(bataille.VuesPlateau[3]).Contains(t5, p14);
            Check.That(bataille.VuesPlateau[4]).Contains(c4, c3);
            Check.That(bataille.VuesPlateau[5]).Contains(t2, c2);
            Check.That(bataille.VuesPlateau[6]).Contains(c4, p13, c3, p3);
            Check.That(bataille.VuesPlateau[7]).Contains(c4, p3, c3, p13);

            Check.That(bataille.Joueurs[0].Paquet).HasSize(0);
            Check.That(bataille.Joueurs[1].Paquet).HasSize(0);
            Check.That(bataille.Joueurs[2].Paquet).HasSize(9);
            Check.That(bataille.Joueurs[2].Paquet).Contains(p2, c3, c4, p3, c2, t5, p13, p14, t2);

            bool gameOver = bataille.JeuEnded(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(bataille.Joueurs[1]);
        }


        /// <summary>
        /// Build a bataille's initial state.
        /// </summary>
        /// <param name="numberOfJoueurs"></param>
        /// <param name="distribution">Represent a stubbed distribution of cartes.
        /// By Joueur, we have a list of Paquet. Hence a list of cartes list
        /// </param>
        /// <returns></returns>
        private static Bataille BuildGame(int numberOfJoueurs, IList<List<Carte>> distribution)
        {
            var joueurs = Enumerable.Range(0, count: 2).Select(x => new Joueur(x)).ToList();
            
            for (int i = 0; i < numberOfJoueurs; i++)
            {
                joueurs[i].Paquet = new Paquet(distribution[i]);
            }

            IDistributeCartes distributeCartes = Substitute.For<IDistributeCartes>();
            distributeCartes.TotalNumberOfCartes.Returns(distribution.SelectMany(x => x).Count());
            distributeCartes.DistributeCartes().Returns(joueurs);

            var game = new Bataille(distributeCartes);
            
            return game;
        }
    }
}