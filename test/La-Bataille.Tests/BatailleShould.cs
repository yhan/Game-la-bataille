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

            bataille.Start();

            Check.That(bataille.VuesPlateau).HasSize(1);
            Check.That(bataille.VuesPlateau[0]).Contains(dixTrefle, piqueValet);

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
            var t5 = 5.AsTrefles();
            var t4 = 4.AsTrefles();
            var p2 = 2.AsPique();
            var c3 = 3.AsCarreaux();
            var c2 = 2.AsCarreaux();
            var p3 = 3.AsPique();
            var bataille = BuildGame(2, new List<List<Carte>>
            {
                new List<Carte> {p2, c3, t4},
                new List<Carte> {p3, c2, t5}
            });

            bataille.Start();

            Check.That(bataille.VuesPlateau).HasSize(5);
            Check.That(bataille.VuesPlateau[0]).Contains(t5, t4);
            Check.That(bataille.VuesPlateau[1]).Contains(c2, c3);
            Check.That(bataille.VuesPlateau[2]).Contains(p3, p2);
            Check.That(bataille.VuesPlateau[3]).Contains(t5, c3);
            Check.That(bataille.VuesPlateau[4]).Contains(c2, t4);


            Check.That(bataille.Joueurs[0].Paquet).HasSize(0);
            Check.That(bataille.Joueurs[1].Paquet).HasSize(6);
            Check.That(bataille.Joueurs[1].Paquet).Contains(t5, t4, p2, c3, c2, p3);
            
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

            IShuffle shuffle = Substitute.For<IShuffle>();
            shuffle.TotalNumberOfCartes.Returns(distribution.SelectMany(x => x).Count());
            shuffle.DistributeCartes().Returns(joueurs);

            var game = new Bataille(shuffle);
            
            return game;
        }
    }
}