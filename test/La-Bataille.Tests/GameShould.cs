using System;
using System.Collections;
using System.Collections.Generic;
using La_Bataille;
using NFluent;
using NUnit.Framework;

namespace Tests
{

    [TestFixture]
    public class CompetitionShould : GameShould
    {
        [Test]
        public void Can_play_n_rounds_and_rank_players()
        {
            var competition = new Competition(new[]
            {
                //#1 - winner player 0
                GameBuilder.BuildGame(new List<List<Card>>
                {
                    new List<Card> {s14, d8, s5, s3, d2, c6},
                    new List<Card> {s11, c9, h5, d4, s2, c5} 
                }),
                GameBuilder.BuildGame(distribution: new List<List<Card>>
                {
                    //#3 -winner player 1
                    new List<Card> {s2, d3, c4},
                    new List<Card> {s3, d2, c5}
                }),
                GameBuilder.BuildGame(new List<List<Card>>
                {
                    //#4 -winner player 0
                    new List<Card> {d8, s3, d2, c6},
                    new List<Card> {c7, d4, s2, c5}
                })
            });

            Ranking ranking = competition.Play();
            Check.That(ranking).HasSize(3);
        }
    }

    

    [TestFixture]
    public class GameShould
    {
        #region cards

        protected readonly Card d2 = 2.AsDiamond();
        protected readonly Card d3 = 3.AsDiamond();
        protected readonly Card d4 = 4.AsDiamond();
        protected readonly Card d7 = 7.AsDiamond();
        protected readonly Card d8 = 8.AsDiamond();
        protected readonly Card d11 = 11.AsDiamond();
        protected readonly Card d14 = 14.AsDiamond();

        protected readonly Card s2 = 2.AsSpade();
        protected readonly Card s3 = 3.AsSpade();
        protected readonly Card s5 = 5.AsSpade();
        protected readonly Card s7 = 7.AsSpade();
        protected readonly Card s8 = 8.AsSpade();
        protected readonly Card s11 = 11.AsSpade();
        protected readonly Card s13 = 13.AsSpade();
        protected readonly Card s14 = 14.AsSpade();

        protected readonly Card c2 = 2.AsClub();
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
        protected readonly Card h5 = 5.AsHeart();
        protected readonly Card h7 = 7.AsHeart();
        protected readonly Card h8 = 8.AsHeart();
        protected readonly Card h9 = 9.AsHeart();
        protected readonly Card h11 = 11.AsHeart();
        protected readonly Card h14 = 14.AsHeart();

            #endregion
        
        [Test]
        public void Drop_cards_when_battle_players_can_no_more_put_FaceUp_cards_on_the_table()
        {
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card>{h8, s14, d7},
                new List<Card>{h9, h14, c10},
                new List<Card>{d2, d3, d11}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            var gameOver = game.Play(NullShuffle.Instance);
            Check.That(game.TableViewsHistory).HasSize(3);

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            var winner = ((HasWinner) gameOver).Winner;
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
            { //#2
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
            var game = GameBuilder.BuildGame(distribution: new List<List<Card>>
            {
                //#3
                new List<Card> {s2, d3, c4},
                new List<Card> {s3, d2, c5}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            var gameOver = game.Play(NullShuffle.Instance);

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
                //#4
                new List<Card> {d8, s3, d2, c6},
                new List<Card> {c7, d4, s2, c5}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            var gameOver = game.Play(NullShuffle.Instance);

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
        public void Can_find_a_winner_with_two_iterations_battles()
        {
            //#1
            var game = GameBuilder.BuildGame(new List<List<Card>>
            {
                new List<Card> {s14, d8, s5, s3, d2, c6},
                new List<Card> {s11, c9, h5, d4, s2, c5}
            }); 
            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];

            var gameOver = game.Play(NullShuffle.Instance);

            Check.That(game.TableViewsHistory).HasSize(6);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(c6.FaceUp(player1), c5.FaceUp(player2));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d2.FaceUp(player1), s2.FaceUp(player2));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(s3.FaceDown(player1), d4.FaceDown(player2));
            Check.That(game.TableViewsHistory[3]).IsEquivalentTo(s5.FaceUp(player1), h5.FaceUp(player2));
            Check.That(game.TableViewsHistory[4]).IsEquivalentTo(d8.FaceDown(player1), c9.FaceDown(player2));
            Check.That(game.TableViewsHistory[5]).IsEquivalentTo(s14.FaceUp(player1), s11.FaceUp(player2));

            Check.That(gameOver).IsInstanceOf<HasWinner>();
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[0]);

            Check.That(game.Players[0].CardStack).HasSize(12);
            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[0].CardStack).IsEquivalentTo(s14, d8, s5, s3, d2, c6, s11, c9, h5, d4, s2, c5);
        }

        [Test]
        public void Can_find_winner_When_battle_happened_recursively_and_in_the_end_all_players_have_no_more_cards_except_for_the_winner()
        {
            var game = GameBuilder.BuildGame(distribution: new List<List<Card>>
            {
                new List<Card> {s2, d3, d4},
                new List<Card> {s3, d2, c5},
                new List<Card> {s13, s14, c2}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];

            var gameOver = game.Play(NullShuffle.Instance);

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
            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[2]);


            Check.That(game.Players[0].CardStack).HasSize(0);
            Check.That(game.Players[1].CardStack).HasSize(0);
            Check.That(game.Players[2].CardStack).HasSize(9);
            Check.That(game.Players[2].CardStack).IsEquivalentTo(s2, d3, d4, s3, d2, c5, s13, s14, c2);
        }


        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_cards_at_the_same_time()
        {
            var game = GameBuilder.BuildGame(distribution: new List<List<Card>>
            {
                new List<Card>{d14, d8, d7},
                new List<Card>{s14, s8, s7},
                new List<Card>{c14, c8, c7},
                new List<Card>{h14, h8, h7}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];
            var player4 = players[3];


            var gameOver = game.Play(NullShuffle.Instance);
            Check.That(game.TableViewsHistory).HasSize(3);
            Check.That(game.TableViewsHistory[0]).IsEquivalentTo(d7.FaceUp(player1), s7.FaceUp(player2), c7.FaceUp(player3), h7.FaceUp(player4));
            Check.That(game.TableViewsHistory[1]).IsEquivalentTo(d8.FaceDown(player1), s8.FaceDown(player2), c8.FaceDown(player3), h8.FaceDown(player4));
            Check.That(game.TableViewsHistory[2]).IsEquivalentTo(d14.FaceUp(player1), s14.FaceUp(player2), c14.FaceUp(player3), h14.FaceUp(player4));

            Check.That(game.DroppedCards).HasSize(12);
            Check.That(gameOver).IsInstanceOf<Draw>();
        }


        [Test]
        public void End_the_game_by_a_Draw_When_the_last_survivors_have_no_more_enough_cards_to_battle()
        {
            var game = GameBuilder.BuildGame(distribution: new List<List<Card>>
            {
                new List<Card>{d11, d14, d8, d7},
                new List<Card>{s11, s14, s8, s7},
                new List<Card>{c11, c14, c8, c7},
                new List<Card>{h11, h14, h8, h7}
            });

            var players = game.Players;
            var player1 = players[0];
            var player2 = players[1];
            var player3 = players[2];
            var player4 = players[3];


            var gameOver = game.Play(NullShuffle.Instance);
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
                new List<Card> {h2, 5.AsHeart(), 7.AsHeart(), 3.AsSpade()},
                new List<Card> {3.AsHeart(), 10.AsClub(), 7.AsSpade(), 4.AsHeart()},
                new List<Card> {14.AsDiamond(), 11.AsDiamond(), 2.AsSpade(), 5.AsClub()}
            });
            var gameOver = game.Play(NullShuffle.Instance);

            Check.That(((HasWinner)gameOver).Winner).IsEqualTo(game.Players[2]);
        }
    }
}