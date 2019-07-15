using System.Collections.Generic;
using System.Linq;
using NFluent;
using NFluent.Helpers;

namespace LaBataille
{
    public class Game
    {
        private readonly IDistributeCards _distributor;

        public Game(IDistributeCards distributor)
        {
            _distributor = distributor;
            DistributedCardsSize = _distributor.DistributedCardsSize;
        }

        public int DistributedCardsSize { get; }

        public List<Player> Players { get; private set; }

        public List<View> TableViewsHistory { get; } = new List<View>();

        public List<Card> DroppedCards { get;  } = new List<Card>();


        public IAmTheGameOver Play(IShuffle shuffle)
        {
            Players = _distributor.Distribute();

            Player winner = null;
            int iterations = 0;
            int iterationsAfterReShuffle = 0;
            bool reShuffled = false;
            while (!GameOver(ref winner))
            {
                if (ShouldContinue(ref iterations, ref reShuffled, ref iterationsAfterReShuffle, out IAmTheGameOver draw))
                {
                    continue;
                }
                if (draw is Draw)
                {
                    return draw;
                }

                var takes = Players.TakeOneCardEach(Visibility.FaceUp);

                TableViewsHistory.Add(takes.BuildView());

                var playerOfStrongestTake = takes.StrongestPlayerIfExit();
                if (playerOfStrongestTake != null)
                {
                    playerOfStrongestTake.Gather(takes);
                    continue;
                }

                RunBattleIfNecessary(takes);
                
                BuildDroppedCards(takes);
            }

            if (winner == null)
            {
                return Draw.Instance;
            }

            Check.That(winner.CardStack.Size + DroppedCards.Count == _distributor.DistributedCardsSize).IsTrue();

            return new HasWinner(winner);
        }

        private bool ShouldContinue(ref int iterations, ref bool reShuffled, ref int iterationsAfterReShuffle, out IAmTheGameOver draw)
        {
            if (iterations == 1000 && !reShuffled)
            {
                foreach (var survivor1000Iterations in Players.Where(p => p.HasCards()))
                {
                    survivor1000Iterations.CardStack.Shuffle();
                }

                reShuffled = true;
                draw = new NullDraw("ReShuffle");
                return true;
            }

            if (iterationsAfterReShuffle == 1000)
            {
                draw = new Draw($"After {iterations} iterations and After ReShuffled {iterationsAfterReShuffle} iterations, still no winner. Declare this game as a DRAW");
                return false;
            }

            iterations++;
            if (reShuffled)
            {
                iterationsAfterReShuffle++;
            }

            draw = new NullDraw("Continuation when iterations < 1000");
            return false;
        }

        private bool GameOver(ref Player player)
        {
            if (DroppedCards.Count == _distributor.DistributedCardsSize)
            {
                player = null;
                return true; // draw
            }

            foreach (var player1 in Players)
            {
                if (player1.CardStack.Size + DroppedCards.Count == _distributor.DistributedCardsSize)
                {
                    player = player1;
                    return true; // has a winner
                }
            }

            player = null;
            return false;
        }

        private void RunBattleIfNecessary(List<Take> takes)
        {
            var numberOfPlayersInTheGame = takes.Count;
            var competitors = new List<Player>();

            while (NeedBattle(takes.KeepTheLast(numberOfPlayersInTheGame), ref competitors))
            {
                numberOfPlayersInTheGame = competitors.Count;

                var competitorsStillHavingCards = competitors.WhoHaveCards();
                if (competitorsStillHavingCards.Length <= 1)
                {
                    takes.Drop();
                }

                if (competitorsStillHavingCards.Length == 1)
                {
                    //competitorsStillHavingCards[0].Gather(takes);

                    break;
                }
                
                // face down cards
                var faceDownTakes = competitors.TakeOneCardEach(Visibility.FaceDown);
                if (!faceDownTakes.Any())
                {
                    break;
                }
                TableViewsHistory.Add(faceDownTakes.BuildView());
                takes.AddRange(faceDownTakes);

                // face up cards
                var faceUpTakes = competitors.TakeOneCardEach(Visibility.FaceUp);
                if (!faceUpTakes.Any())
                {
                    takes.Drop();
                    break;
                }

                if (faceUpTakes.Count == 1)
                {
                    takes.Drop();
                    faceUpTakes[0].Player.Gather(faceUpTakes);
                    break;
                }

                TableViewsHistory.Add(faceUpTakes.BuildView());
                takes.AddRange(faceUpTakes);
                if (faceUpTakes.AllEqual())
                {
                    continue;
                }

                // Find the competitor having the strongest card, and only he has that card
                var strongestPlayer = faceUpTakes.StrongestPlayerIfExit();

                if (strongestPlayer != null)
                {
                    strongestPlayer.Gather(takes.Where(x => !x.Dropped));

                    break;
                }
            }
        }

        private void BuildDroppedCards(IEnumerable<Take> takes)
        {
            DroppedCards.AddRange(takes.Where(x => x.Dropped).Select(t => t.Card));
        }

        private static bool NeedBattle(IReadOnlyList<Take> takes, ref List<Player> competitors)
        {
            var max2 = takes.Max();
            var equalAndStrongest = takes.Where(t => t.CompareTo(max2) == 0);

            competitors = equalAndStrongest.Select(t => t.Player).ToList();

            return competitors.Count > 1;




            if (!takes.Any())
            {
                competitors = null;
                return false;
            }

            Take max = takes[0];
            competitors = new List<Player>();

            foreach (var take in takes)
            {
                var compareTo = take.CompareTo(max);
                if (compareTo > 0)
                {
                    competitors.Clear();

                    max = take;
                    competitors.Add(take.Player);
                }
                else if (compareTo == 0)
                {
                    competitors.Add(take.Player);
                }
            }

            return competitors.Count > 1;
        }
    }
}