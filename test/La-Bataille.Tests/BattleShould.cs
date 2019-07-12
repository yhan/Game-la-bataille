using System.Collections.Generic;
using System.Linq;

using La_Bataille;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BattleShould
    {
        private readonly Card d2 = 2.AsDiamond();
        private readonly Card d3 = 3.AsDiamond();
        private readonly Card d4 = 4.AsDiamond();
        private readonly Card d8 = 8.AsDiamond();

        private readonly Card s2 = 2.AsSpade();
        private readonly Card s3 = 3.AsSpade();
        private readonly Card s5 = 5.AsSpade();
        private readonly Card s8 = 8.AsSpade();
        private readonly Card s11 = 11.AsSpade();
        private readonly Card s13 = 13.AsSpade();
        private readonly Card s14 = 14.AsSpade();

        private readonly Card c2 = 2.AsClub();
        private readonly Card c4 = 4.AsClub();
        private readonly Card c5 = 5.AsClub();
        private readonly Card c6 = 6.AsClub();
        private readonly Card c7 = 7.AsClub();
        private readonly Card c8 = 8.AsClub();
        private readonly Card c9 = 9.AsClub();
        private readonly Card c10 = 10.AsClub();


        private readonly Card h5 = 5.AsHeart();


        [Test]
        public void Can_play_la_premiere_levee()
        { 
            var battle = BuildGame(2, new List<List<Card>>
            {
                new List<Card> {c10},
                new List<Card> {s11}
            });

            battle.Start(NullShuffle.Instance);

            Check.That(battle.TableViewsHistory).HasSize(1);
            Check.That(battle.TableViewsHistory[0].AsPureCartes()).Contains(c10, s11);

            Check.That(battle.Players[0].CardStack).HasSize(0);

            Check.That(battle.Players[1].CardStack).HasSize(2);
            Check.That(battle.Players[1].CardStack).Contains(c10, s11);


            bool gameOver = battle.IsGameOver(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(battle.Players[1]);
        }

        [Test]
        public void Can_play_n_levees_to_the_end_of_game()
        {
            var battle = BuildGame(numberOfJoueurs: 2, distribution: new List<List<Card>>
            {
                new List<Card> {s2, d3, c4},
                new List<Card> {s3, d2, c5}
            });

            battle.Start(NullShuffle.Instance);

            Check.That(battle.TableViewsHistory).HasSize(5);
            Check.That(battle.TableViewsHistory[0]).Contains(c5.FaceUp(), c4.FaceUp());
            Check.That(battle.TableViewsHistory[1]).Contains(d2.FaceUp(), d3.FaceUp());
            Check.That(battle.TableViewsHistory[2]).Contains(s3.FaceUp(), s2.FaceUp());
            Check.That(battle.TableViewsHistory[3]).Contains(c5.FaceUp(), d3.FaceUp());
            Check.That(battle.TableViewsHistory[4]).Contains(d2.FaceUp(), c4.FaceUp());


            Check.That(battle.Players[0].CardStack).HasSize(0);
            Check.That(battle.Players[1].CardStack).HasSize(6);
            Check.That(battle.Players[1].CardStack).Contains(c5, c4, s2, d3, d2, s3);
            
            bool gameOver = battle.IsGameOver(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(battle.Players[1]);
        }
        
        [Test]
        public void Can_find_gagnant_with_only_one_battle()
        {
            var battle = BuildGame(2, new List<List<Card>>
            {
                new List<Card> {d8, s3, d2, c6},
                new List<Card> {c7, d4, s2, c5}
            });

            battle.Start(NullShuffle.Instance);

            Check.That(battle.TableViewsHistory).HasSize(4);
            Check.That(battle.TableViewsHistory[0]).Contains(c6.FaceUp(), c5.FaceUp());
            Check.That(battle.TableViewsHistory[1]).Contains(d2.FaceUp(), s2.FaceUp());
            Check.That(battle.TableViewsHistory[2]).Contains(s3.FaceDown(), d4.FaceDown());
            Check.That(battle.TableViewsHistory[3]).Contains(d8.FaceUp(), c7.FaceUp());
            
            bool gameOver = battle.IsGameOver(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(battle.Players[0]);

            Check.That(battle.Players[1].CardStack).HasSize(0);
            Check.That(battle.Players[0].CardStack).HasSize(8);
            Check.That(battle.Players[0].CardStack).Contains(d8, s3, d2, c6, c7, d4, s2, c5);
        }
        
        
        [Test]
        public void Can_find_gagnant_with_two_iterations_battles()
        {
            var battle = BuildGame(2, new List<List<Card>>
            {
                new List<Card> {s14, d8, s5, s3, d2, c6},
                new List<Card> {s11, c9, h5, d4, s2, c5}
            });

            battle.Start(NullShuffle.Instance);

            Check.That(battle.TableViewsHistory).HasSize(6);
            Check.That(battle.TableViewsHistory[0]).Contains(c6.FaceUp(), c5.FaceUp());
            Check.That(battle.TableViewsHistory[1]).Contains(d2.FaceUp(), s2.FaceUp());
            Check.That(battle.TableViewsHistory[2]).Contains(s3.FaceDown(), d4.FaceDown());
            Check.That(battle.TableViewsHistory[3]).Contains(s5.FaceUp(), h5.FaceUp());
            Check.That(battle.TableViewsHistory[4]).Contains(d8.FaceDown(), c9.FaceDown());
            Check.That(battle.TableViewsHistory[5]).Contains(s14.FaceUp(), s11.FaceUp());
            
            bool gameOver = battle.IsGameOver(out var vainqueur);
            Check.That(gameOver).IsTrue();
            Check.That(vainqueur).IsEqualTo(battle.Players[0]);

            Check.That(battle.Players[1].CardStack).HasSize(0);
            Check.That(battle.Players[0].CardStack).HasSize(12);
            Check.That(battle.Players[0].CardStack).Contains(s14, d8, s5, s3, d2, c6, s11, c9, h5, d4, s2, c5);
        }
        
        [Test]
        public void Can_find_winner_When_battle_happened_recursively_and_in_the_end_all_players_but_the_winner_have_no_more_cards()
        {
            var battle = BuildGame(numberOfJoueurs: 3, distribution: new List<List<Card>>
            {
                new List<Card> {s2, d3, d4},
                new List<Card> {s3, d2, c5},
                new List<Card> {s13, s14, c2}
            });

            var shuffle = Substitute.For<IShuffle>();

            battle.Start(shuffle);

            Check.That(battle.TableViewsHistory).HasSize(8);
            Check.That(battle.TableViewsHistory[0]).Contains(d4.FaceUp(), c5.FaceUp(), c2.FaceUp());
            Check.That(battle.TableViewsHistory[1]).Contains(d2.FaceUp(), d3.FaceUp(), s14.FaceUp());
            Check.That(battle.TableViewsHistory[2]).Contains(s3.FaceUp(), s2.FaceUp(), s13.FaceUp());
            Check.That(battle.TableViewsHistory[3]).Contains(c5.FaceUp(), s14.FaceUp());
            Check.That(battle.TableViewsHistory[4]).Contains(d4.FaceUp(), d3.FaceUp());
            Check.That(battle.TableViewsHistory[5]).Contains(c2.FaceUp(), d2.FaceUp());
            Check.That(battle.TableViewsHistory[6]).Contains(d4.FaceDown(), s13.FaceDown());
            Check.That(battle.TableViewsHistory[7]).Contains(s3.FaceUp(), d3.FaceUp());

            Check.That(battle.Players[0].CardStack).HasSize(0);
            Check.That(battle.Players[1].CardStack).HasSize(0);
            Check.That(battle.Players[2].CardStack).HasSize(9);
            Check.That(battle.Players[2].CardStack).Contains(s2, d3, d4, s3, d2, c5, s13, s14, c2);

            bool gameOver = battle.IsGameOver(out var winner);
            Check.That(gameOver).IsTrue();
            Check.That(winner).IsEqualTo(battle.Players[2]);
        }


        /// <summary>
        /// Build a battle's initial state.
        /// </summary>
        /// <param name="numberOfJoueurs"></param>
        /// <param name="distribution">Represent a stubbed distribution of cartes.
        /// By Player, we have a list of CardStack. Hence a list of cartes list
        /// </param>
        /// <returns></returns>
        private static Game BuildGame(int numberOfJoueurs, IList<List<Card>> distribution)
        {
            var joueurs = Enumerable.Range(0, count: numberOfJoueurs).Select(x => new Player(x)).ToList();
            
            for (int i = 0; i < numberOfJoueurs; i++)
            {
                joueurs[i].CardStack = new CardStack(distribution[i]);
            }

            IDistributeCards distributeCards = Substitute.For<IDistributeCards>();
            distributeCards.TotalNumberOfCards.Returns(distribution.SelectMany(x => x).Count());
            distributeCards.Distribute().Returns(joueurs);

            var game = new Game(distributeCards);
            
            return game;
        }
    }
}