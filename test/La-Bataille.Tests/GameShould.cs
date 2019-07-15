using System;
using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

namespace LaBataille.Tests
{
    [TestFixture]
    public class GameShould
    {
        #region cards

        protected readonly Card d2 = 2.AsDiamond();
        protected readonly Card d3 = 3.AsDiamond();
        protected readonly Card d4 = 4.AsDiamond();
        protected readonly Card d5 = 5.AsDiamond();
        protected readonly Card d6 = 6.AsDiamond();
        protected readonly Card d7 = 7.AsDiamond();
        protected readonly Card d8 = 8.AsDiamond();
        protected readonly Card d10 = 10.AsDiamond();
        protected readonly Card d11 = 11.AsDiamond();
        protected readonly Card d12 = 12.AsDiamond();
        protected readonly Card d14 = 14.AsDiamond();

        protected readonly Card s2 = 2.AsSpade();
        protected readonly Card s3 = 3.AsSpade();
        protected readonly Card s5 = 5.AsSpade();
        protected readonly Card s6 = 6.AsSpade();
        protected readonly Card s7 = 7.AsSpade();
        protected readonly Card s8 = 8.AsSpade();
        protected readonly Card s11 = 11.AsSpade();
        protected readonly Card s12 = 12.AsSpade();
        protected readonly Card s13 = 13.AsSpade();
        protected readonly Card s14 = 14.AsSpade();

        protected readonly Card c2 = 2.AsClub();
        protected readonly Card c3 = 3.AsClub();
        protected readonly Card c4 = 4.AsClub();
        protected readonly Card c5 = 5.AsClub();
        protected readonly Card c6 = 6.AsClub();
        protected readonly Card c7 = 7.AsClub();
        protected readonly Card c8 = 8.AsClub();
        protected readonly Card c9 = 9.AsClub();
        protected readonly Card c10 = 10.AsClub();
        protected readonly Card c11 = 11.AsClub();
        protected readonly Card c14 = 14.AsClub();


        protected readonly Card h2 = 2.AsHeart();
        protected readonly Card h3 = 3.AsHeart();
        protected readonly Card h4 = 4.AsHeart();
        protected readonly Card h5 = 5.AsHeart();
        protected readonly Card h6 = 6.AsHeart();
        protected readonly Card h7 = 7.AsHeart();
        protected readonly Card h8 = 8.AsHeart();
        protected readonly Card h9 = 9.AsHeart();
        protected readonly Card h10 = 10.AsHeart();
        protected readonly Card h11 = 11.AsHeart();
        protected readonly Card h12 = 12.AsHeart();
        protected readonly Card h14 = 14.AsHeart();

        #endregion


        /// <summary>
        ///  When we battle h10 and d10, the very first uncovered cards h6, d4
        ///  should be gathered by the final winner of battle: the card holder of d12
        /// 
        /// </summary>
        [Test]
        public void Should_take_other_cards_in_the_same_range_When_face_up_cards_get_a_stronger_one2()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{s13, d2,  s2,  d5,  h6},
                new List<Card>{s8,  h9,  h10, d7,  s11},
                new List<Card>{d12, d14, d10, s12, d11},
                new List<Card>{d3,  h3,  d14, s8,  d4}

            }, PlayersBuilder.BuildPlayers(4, new CardsShufflerForTest()));

            var gameOver = game.Play();

            var hasWinner = (HasWinner)gameOver;
            var player2 = game.Players[2];
            Check.That(hasWinner.Winner).IsEqualTo(player2);

            foreach (var view in game.TableViewsHistory)
            {
                Console.WriteLine(view);
            }

            Check.That(player2.CardStack.Size).IsEqualTo(4 * 5);
            Check.That(game.DroppedCards).HasSize(0);
        }

        /// <summary>
        ///  When we battle s11 and d11, the very first uncovered cards h6
        ///  should be gathered by the final winner of battle: the card holder of d4
        /// 
        /// </summary>
        [Test]
        public void Should_take_other_cards_in_the_same_range_When_face_up_cards_get_a_stronger_one()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{s2, d5, h6},
                new List<Card>{s3, d7, s11},
                new List<Card>{d4, h12, d11}

            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));

            var gameOver = game.Play();

            var hasWinner = (HasWinner)gameOver;
            var player2 = game.Players[2];
            Check.That(hasWinner.Winner).IsEqualTo(player2);

            Check.That(player2.CardStack.Size).IsEqualTo(9);
        }

        [Test]
        public void Should_have_correct_number_of_won_cards_and_dropped_cards()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{s13, s13, d2,  c4,  d6,  s3},
                new List<Card>{s6,  s5,  h9,  h4,  d7,  h3},
                new List<Card>{s7,  d5,  d14, d4,  s12, d3},
                new List<Card>{s2,  d8,  h7,  s11, s8,  c3}

            }, PlayersBuilder.BuildPlayers(4, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var hasWinner = (HasWinner)gameOver;
            Check.That(game.DroppedCards.Count + hasWinner.Winner.CardStack.Size).IsEqualTo(6 * 4);
        }


        [Test]
        public void Should_continue_battle_if_faceUp_cards_are_all_identical()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{s7, d2, h8, s11, s2},
                new List<Card>{s6, d3, s8, s13, d2}

            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));

            var gameOver = game.Play();

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            var hasWinner = (HasWinner)gameOver;

            var player0 = game.Players[0];
            Check.That(hasWinner.Winner).IsEqualTo(player0);

            Check.That(player0.CardStack.Size).IsEqualTo(10);
            Check.That(game.DroppedCards.Count).IsEqualTo(0);
        }


        [Test]
        public void Drop_cards_when_battle_players_can_no_more_put_FaceUp_cards_on_the_table()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{h8, s14, d7},
                new List<Card>{h9, h14, c10},
                new List<Card>{d2, d3, d11}
            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            Check.That(game.TableViewsHistory).HasSize(3);

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            var winner = ((HasWinner)gameOver).Winner;
            Check.That(winner).IsEqualTo(player3);
            Check.That(winner.CardStack).IsEquivalentTo(d7, c10, d11, d2);

            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(player1), c10.FaceUp(player2), d11.FaceUp(player3));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(s14.FaceUp(player1), h14.FaceUp(player2), d3.FaceUp(player3));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(h8.FaceDown(player1), h9.FaceDown(player2));

            Check.That(game.DroppedCards).HasSize(5);
            Check.That(game.DroppedCards).IsEquivalentTo(h8, h9, s14, h14, d3);
        }

        [Test]
        public void Can_play_the_very_first_take()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            { 
                new List<Card> {c10},
                new List<Card> {s11}
            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var player0 = game.Players[0];
            var player1 = game.Players[1];
            Check.That(game.TableViewsHistory).HasSize(1);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c10.FaceUp(player0), s11.FaceUp(player1));

            Check.That(game.Players[0].CardStack).HasSize(0);

            Check.That(game.Players[1].CardStack).HasSize(2);
            Check.That(game.Players[1].CardStack).IsEquivalentTo(c10, s11);


            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[1]);
        }

        [Test]
        public void Can_play_n_takes_to_the_end_of_game()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {s2, d3, c4},
                new List<Card> {s3, d2, c5}
            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));


            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            Check.That(game.TableViewsHistory).HasSize(5);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c4.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d3.FaceUp(player1), d2.FaceUp(player2));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s2.FaceUp(player1), s3.FaceUp(player2));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d3.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d2.FaceUp(player1), c4.FaceUp(player2));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[1]);

            Check.That(game.Players[0].CardStack).HasSize(0);
            Check.That(game.Players[1].CardStack).HasSize(6);
            Check.That(game.Players[1].CardStack).IsEquivalentTo(c5, c4, s2, d3, d2, s3);
        }

        [Test]
        public void Can_find_a_winner_with_only_one_battle()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {d8, s3, d2, c6},
                new List<Card> {c7, d4, s2, c5}
            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));


            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            Check.That(game.TableViewsHistory).HasSize(4);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(player1), s2.FaceUp(player2));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(player1), d4.FaceDown(player2));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d8.FaceUp(player1), c7.FaceUp(player2));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[0]);

            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[0].CardStack).HasSize(8);
            Check.That(game.Players[0].CardStack).IsEquivalentTo(d8, s3, d2, c6, c7, d4, s2, c5);
        }

        [Test]
        public void Find_a_winner_with_only_two_battle_and_2nd_battle_failed_to_play_because_no_more_enough_cards()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {d7, s3, d2, c6},
                new List<Card> {c7, d4, s2, c5}
            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));


            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            Check.That(game.TableViewsHistory).HasSize(4);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(player1), s2.FaceUp(player2));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(player1), d4.FaceDown(player2));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d7.FaceUp(player1), c7.FaceUp(player2));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            var hasWinner = (HasWinner)gameOver;
            Check.That(hasWinner.Winner).IsEqualTo(player1);

            Check.That(player1.CardStack).HasSize(2);
            Check.That(player2.CardStack).HasSize(0);
            Check.That(game.DroppedCards).HasSize(6);
        }


        [Test]
        public void Can_find_a_winner_with_two_iterations_battles()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {s14, d8, s5, s3, d2, c6},
                new List<Card> {s11, c9, h5, d4, s2, c5}
            }, PlayersBuilder.BuildPlayers(2, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            Check.That(game.TableViewsHistory).HasSize(6);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(player1), s2.FaceUp(player2));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(player1), d4.FaceDown(player2));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(s5.FaceUp(player1), h5.FaceUp(player2));
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d8.FaceDown(player1), c9.FaceDown(player2));
            Check.That(game.TableViewsHistory[5]).IsEquivalentTo(s14.FaceUp(player1), s11.FaceUp(player2));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[0]);

            Check.That(player1.CardStack).HasSize(12);
            Check.That(player2.CardStack).HasSize(0);
            Check.That(player1.CardStack).IsEquivalentTo(s14, d8, s5, s3, d2, c6, s11, c9, h5, d4, s2, c5);
        }

        [Test]
        public void Can_find_winner_When_battle_happened_recursively_and_in_the_end_all_players_have_no_more_cards_except_for_the_winner()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {s2, d3, d4},
                new List<Card> {s3, d2, c5},
                new List<Card> {s13, s14, c2}
            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));


            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            Check.That(game.TableViewsHistory).HasSize(8);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d4.FaceUp(player1), c5.FaceUp(player2), c2.FaceUp(player3));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d3.FaceUp(player1), d2.FaceUp(player2), s14.FaceUp(player3));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s2.FaceUp(player1), s3.FaceUp(player2), s13.FaceUp(player3));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(c5.FaceUp(player2), s14.FaceUp(player3));
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d4.FaceUp(player2), d3.FaceUp(player3));
            Check.That(game.TableViewsHistory[5]).IsEquivalentTo(c2.FaceUp(player2), d2.FaceUp(player3));
            Check.That(game.TableViewsHistory[6]).IsEquivalentTo(d4.FaceDown(player2), s13.FaceDown(player3));
            Check.That(game.TableViewsHistory[7]).IsEquivalentTo(s3.FaceUp(player2), d3.FaceUp(player3));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            var winner = ((HasWinner)gameOver).Winner;
            Check.That(winner).IsEqualTo(player3);
            Check.That(winner.CardStack.Size).IsEqualTo(3);

            Check.That(player1.CardStack).HasSize(0);
            Check.That(player2.CardStack).HasSize(0);
            Check.That(player3.CardStack).HasSize(3);
            Check.That(player3.CardStack).IsEquivalentTo(c5, s14, s2);

            Check.That(game.DroppedCards.Count).IsEqualTo(6);
        }

        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_cards_at_the_same_time()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{d14, d8, d7},
                new List<Card>{s14, s8, s7},
                new List<Card>{c14, c8, c7}
            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            Check.That(game.TableViewsHistory).HasSize(3);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(player1), s7.FaceUp(player2), c7.FaceUp(player3));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d8.FaceDown(player1), s8.FaceDown(player2), c8.FaceDown(player3));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(d14.FaceUp(player1), s14.FaceUp(player2), c14.FaceUp(player3));

            Check.That(game.DroppedCards).HasSize(9);
            Check.That(gameOver).IsInstanceOf<Draw>();
        }

        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_enough_cards_to_battle()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{d11, d14, d8, d7},
                new List<Card>{s11, s14, s8, s7},
                new List<Card>{c11, c14, c8, c7},
                new List<Card>{h11, h14, h8, h7}
            }, PlayersBuilder.BuildPlayers(4, new CardsShufflerForTest()));

            var gameOver = game.Play();
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];
            var player4 = players[3];

            Check.That(game.TableViewsHistory).HasSize(4);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(player1), s7.FaceUp(player2), c7.FaceUp(player3), h7.FaceUp(player4));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d8.FaceDown(player1), s8.FaceDown(player2), c8.FaceDown(player3), h8.FaceDown(player4));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(d14.FaceUp(player1), s14.FaceUp(player2), c14.FaceUp(player3), h14.FaceUp(player4));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(d11.FaceDown(player1), s11.FaceDown(player2), c11.FaceDown(player3), h11.FaceDown(player4));

            Check.That(game.DroppedCards).HasSize(16);
            Check.That(gameOver).IsInstanceOf<Draw>();
        }

        [Test]
        public void Should_continue_When_a_player_has_no_more_card_after_a_battle_but_not_all_players()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {h2, h5, h7, s3},
                new List<Card> {h3, c10, s7, h4},
                new List<Card> {d14, d11, s2, c5}
            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));
            var gameOver = game.Play();

            var history = game.TableViewsHistory;
            Check.That(history).HasSize(15);
            var winner = ((HasWinner)gameOver).Winner;
            Check.That(winner).IsEqualTo(game.Players[2]);

            Check.That(winner.CardStack.Size).IsEqualTo(12);
            Check.That(game.DroppedCards).HasSize(0);
        }

        [Test]
        public void Should_continue_When_a_player_has_no_more_card_after_a_battle_but_not_all_players2()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {h5, d6, s7},
                new List<Card> {s6, s8, h7},
                new List<Card> { h4, s3, d2}
            }, PlayersBuilder.BuildPlayers(3, new CardsShufflerForTest()));
            var gameOver = game.Play();

            var player0 = game.Players[0];
            var player1 = game.Players[1];
            var player2 = game.Players[2];

            var history = game.TableViewsHistory;
            Check.That(history).HasSize(5);
            Check.That(history[0]).IsEquivalentTo(s7.FaceUp(player0), h7.FaceUp(player1), d2.FaceUp(player2));
            Check.That(history[1]).IsEquivalentTo(d6.FaceDown(player0), s8.FaceDown(player1));
            Check.That(history[2]).IsEquivalentTo(h5.FaceUp(player0), s6.FaceUp(player1));
            Check.That(history[3]).IsEquivalentTo(s8.FaceUp(player1), s3.FaceUp(player2));
            Check.That(history[4]).IsEquivalentTo(s7.FaceUp(player1), h4.FaceUp(player2));

            var winner = ((HasWinner)gameOver).Winner;
            Check.That(winner).IsEqualTo(game.Players[1]);

            Check.That(winner.CardStack.Size).IsEqualTo(9);
            Check.That(game.DroppedCards).HasSize(0);
        }

    }
}