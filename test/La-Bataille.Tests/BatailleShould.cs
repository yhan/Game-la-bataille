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
            Check.That(bataille.VuesPlateau[0].Cartes).Contains(dixTrefle, piqueValet);

            Check.That(bataille.Joueurs[0].Cartes).HasSize(0);
            Check.That(bataille.Joueurs[0].Cartes).IsOnlyMadeOf(dixTrefle, piqueValet);


            bool gameOver = bataille.JeuEnded(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(bataille.Joueurs[1]);
        }

        [Test]
        public void Can_play_n_levees_to_the_end_of_game()
        {

        }


        private static Bataille BuildGame(int numberOfJoueurs, IList<List<Carte>> distribution)
        {
            var joueurs = Enumerable.Range(0, count: 2).Select(x => new Joueur(x)).ToList();
            
            for (int i = 0; i < numberOfJoueurs; i++)
            {
                joueurs[i].Cartes = distribution[i];
            }

            IShuffle shuffle = Substitute.For<IShuffle>();
            shuffle.TotalNumberOfCartes.Returns(distribution.SelectMany(x => x).Count());
            shuffle.DistributeCartes().Returns(joueurs);

            
            var game = new Bataille(shuffle);
            
            return game;
        }
    }
}