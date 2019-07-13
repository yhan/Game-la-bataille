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
        private readonly Card d7 = 7.AsDiamond();
        private readonly Card d8 = 8.AsDiamond();
        private readonly Card d11 = 11.AsDiamond();
        private readonly Card d14 = 14.AsDiamond();

        private readonly Card s2 = 2.AsSpade();
        private readonly Card s3 = 3.AsSpade();
        private readonly Card s5 = 5.AsSpade();
        private readonly Card s7 = 7.AsSpade();
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
        private readonly Card c11 = 11.AsClub();
        private readonly Card c14 = 14.AsClub();


        private readonly Card h5 = 5.AsHeart();
        private readonly Card h7 = 7.AsHeart();
        private readonly Card h8 = 8.AsHeart();
        private readonly Card h11 = 11.AsHeart();
        private readonly Card h14 = 14.AsHeart();


        [Test]
        public void Can_play_the_very_first_take()
        {
            var game = BuildGame(new List<List<Card>>
            {
                new List<Card> {c10},
                new List<Card> {s11}
            });

            var gameOver = game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(1);
            Check.That(game.TableViewsHistory[0].AsPureCartes()).IsEquivalentTo(c10, s11);

            Check.That(game.Players[0].CardStack).HasSize(0);

            Check.That(game.Players[1].CardStack).HasSize(2);
            Check.That(game.Players[1].CardStack).IsEquivalentTo(c10, s11);


            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[1]);
        }

        [Test]
        public void Can_play_n_takes_to_the_end_of_game()
        {
            var game = BuildGame(distribution: new List<List<Card>>
            {
                new List<Card> {s2, d3, c4},
                new List<Card> {s3, d2, c5}
            });

            var gameOver = game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(5);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c5.FaceUp(), c4.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(), d3.FaceUp());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceUp(), s2.FaceUp());
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(c5.FaceUp(), d3.FaceUp());
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d2.FaceUp(), c4.FaceUp());

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[1]);

            Check.That(game.Players[0].CardStack).HasSize(0);
            Check.That(game.Players[1].CardStack).HasSize(6);
            Check.That(game.Players[1].CardStack).IsEquivalentTo(c5, c4, s2, d3, d2, s3);
        }

        [Test]
        public void Can_find_a_winner_with_only_one_battle()
        {
            var game = BuildGame(new List<List<Card>>
            {
                new List<Card> {d8, s3, d2, c6},
                new List<Card> {c7, d4, s2, c5}
            });

            var gameOver = game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(4);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(), c5.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(), s2.FaceUp());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(), d4.FaceDown());
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d8.FaceUp(), c7.FaceUp());

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[0]);

            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[0].CardStack).HasSize(8);
            Check.That(game.Players[0].CardStack).IsEquivalentTo(d8, s3, d2, c6, c7, d4, s2, c5);
        }

        [Test]
        public void Can_find_a_winner_with_two_iterations_battles()
        {
            var game = BuildGame(new List<List<Card>>
            {
                new List<Card> {s14, d8, s5, s3, d2, c6},
                new List<Card> {s11, c9, h5, d4, s2, c5}
            });

            var gameOver= game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(6);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(), c5.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(), s2.FaceUp());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(), d4.FaceDown());
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(s5.FaceUp(), h5.FaceUp());
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d8.FaceDown(), c9.FaceDown());
            Check.That(game.TableViewsHistory[5]).IsEquivalentTo(s14.FaceUp(), s11.FaceUp());

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[0]);

            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[0].CardStack).HasSize(12);
            Check.That(game.Players[0].CardStack).IsEquivalentTo(s14, d8, s5, s3, d2, c6, s11, c9, h5, d4, s2, c5);
        }

        [Test]
        public void Can_find_winner_When_battle_happened_recursively_and_in_the_end_all_players_have_no_more_cards_except_for_the_winner()
        {
            var game = BuildGame(distribution: new List<List<Card>>
            {
                new List<Card> {s2, d3, d4},
                new List<Card> {s3, d2, c5},
                new List<Card> {s13, s14, c2}
            });

            var gameOver= game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(8);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d4.FaceUp(), c5.FaceUp(), c2.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(), d3.FaceUp(), s14.FaceUp());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceUp(), s2.FaceUp(), s13.FaceUp());
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(c5.FaceUp(), s14.FaceUp());
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d4.FaceUp(), d3.FaceUp());
            Check.That(game.TableViewsHistory[5]).IsEquivalentTo(c2.FaceUp(), d2.FaceUp());
            Check.That(game.TableViewsHistory[6]).IsEquivalentTo(d4.FaceDown(), s13.FaceDown());
            Check.That(game.TableViewsHistory[7]).IsEquivalentTo(s3.FaceUp(), d3.FaceUp());

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[2]);


            Check.That(game.Players[0].CardStack).HasSize(0);
            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[2].CardStack).HasSize(9);
            Check.That(game.Players[2].CardStack).IsEquivalentTo(s2, d3, d4, s3, d2, c5, s13, s14, c2);
        }


        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_cards_at_the_same_time()
        {
            var game = BuildGame(distribution: new List<List<Card>>
            {
                new List<Card>{d14, d8, d7},
                new List<Card>{s14, s8, s7},
                new List<Card>{c14, c8, c7},
                new List<Card>{h14, h8, h7}
            });

            var gameOver = game.Play(NullShuffle.Instance);
            Check.That(game.TableViewsHistory).HasSize(3);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(), s7.FaceUp(), c7.FaceUp(), h7.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d8.FaceDown(), s8.FaceDown(), c8.FaceDown(), h8.FaceDown());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(d14.FaceUp(), s14.FaceUp(), c14.FaceUp(), h14.FaceUp());


            Check.That(gameOver).IsInstanceOf<Draw>();
        }


        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_enough_cards_to_battle()
        {
            var game = BuildGame(distribution: new List<List<Card>>
            {
                new List<Card>{d11, d14, d8, d7},
                new List<Card>{s11, s14, s8, s7},
                new List<Card>{c11, c14, c8, c7},
                new List<Card>{h11, h14, h8, h7}
            });

            var gameOver = game.Play(NullShuffle.Instance);
            Check.That(game.TableViewsHistory).HasSize(4);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(), s7.FaceUp(), c7.FaceUp(), h7.FaceUp());
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d8.FaceDown(), s8.FaceDown(), c8.FaceDown(), h8.FaceDown());
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(d14.FaceUp(), s14.FaceUp(), c14.FaceUp(), h14.FaceUp());
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d11.FaceDown(), s11.FaceDown(), c11.FaceDown(), h11.FaceDown());


            Check.That(gameOver).IsInstanceOf<Draw>();
        }

        /// <summary>
        /// Build a battle's initial state.
        /// </summary>
        /// <param name="distribution">Represent a stubbed distribution of cartes.
        ///     By Player, we have a list of CardStack. Hence a list of cartes list
        /// </param>
        /// <returns></returns>
        private static Game BuildGame(IList<List<Card>> distribution)
        {
            var numberOfJoueurs = distribution.Count;

            var players = Enumerable.Range(0, count: numberOfJoueurs).Select(x => new Player(x)).ToList();

            for (int i = 0; i < numberOfJoueurs; i++)
            {
                players[i].CardStack = new CardStack(distribution[i]);
            }

            IDistributeCards cardsDistributor = Substitute.For<IDistributeCards>();
            cardsDistributor.DistributedCardsSize.Returns(distribution.SelectMany(x => x).Count());
            cardsDistributor.Distribute().Returns(players);

            var game = new Game(cardsDistributor);

            return game;
        }
    }
}