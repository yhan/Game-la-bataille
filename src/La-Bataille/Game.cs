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

        }

        public List<Player> Players { get; private set; }

        public List<View> TableViewsHistory { get; } = new List<View>();

        public List<Card> DroppedCards { get; set; } = new List<Card>();


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

        public IAmTheGameOver Play(IShuffle shuffle)
        {
            Players = _distributor.Distribute();

            Player winner = null;

            while (!GameOver(ref winner))
            {
                List<Take> takes = Players.TakeOneCardEach(Visibility.FaceUp);

                TableViewsHistory.Add(takes.BuildView());

                Player playerOfStrongestTake = takes.StrongestPlayerIfExit();

                bool highestTakeIsFromBattle = false;

                RunBattleIfNecessary(takes, ref highestTakeIsFromBattle);
                
                BuildDroppedCards(takes);

                if (!highestTakeIsFromBattle)
                {
                    playerOfStrongestTake?.Gather(takes);
                }
            }

            if (winner == null)
            {
                return Draw.Instance;
            }

            Check.That(winner.CardStack.Size + DroppedCards.Count == _distributor.DistributedCardsSize).IsTrue();

            return new HasWinner(winner);
        }

        private void BuildDroppedCards(List<Take> takes)
        {
            DroppedCards.AddRange(takes.Where(x => x.Dropped).Select(t => t.Card));
        }

        private void RunBattleIfNecessary(List<Take> takes, ref bool highestTakeIsFromBattle)
        {
            var numberOfPlayersInTheGame = takes.Count;
            var competitors = new List<Player>();

            while (NeedBattle(takes.KeepTheLast(numberOfPlayersInTheGame), ref competitors))
            {
                var competitorsStillHavingCards = competitors.Where(c => c.HasCards()).ToArray();
                if (competitorsStillHavingCards.Length <= 1)
                {
                    takes.Drop();

                    if (competitorsStillHavingCards.Length == 1)
                    {
                        break;
                    }
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

                TableViewsHistory.Add(faceUpTakes.BuildView());
                takes.AddRange(faceUpTakes);

                // F
                var playerIf = faceUpTakes.StrongestPlayerIfExit();

                if (playerIf != null)
                {
                    playerIf.Gather(takes);

                    highestTakeIsFromBattle = true;
                    break;
                }
            }

            //return winner;
        }

        private static bool NeedBattle(IReadOnlyList<Take> takes, ref List<Player> competitors)
        {
            var competitors2 = new List<Player>();


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