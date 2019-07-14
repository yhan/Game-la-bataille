using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    public class Game
    {
        private readonly IDistributeCards _distributor;

        public Game(IDistributeCards distributor)
        {
            _distributor = distributor;
            Players = _distributor.Distribute();
        }

        public List<Player> Players { get; }

        public List<View> TableViewsHistory { get; } = new List<View>();

        public List<Card> DroppedCards { get; set; } = new List<Card>();


        private bool FoundWinner(ref Player player)
        {
            foreach (var player1 in Players)
            {
                if (player1.CardStack.Size + DroppedCards.Count == _distributor.DistributedCardsSize)
                {
                    player = player1;
                    return true;
                }
            }

            return false;
        }

        public IAmTheGameOver Play(IShuffle shuffle)
        {
            Player winner = null;
            
            while (!FoundWinner(ref winner))
            {
                List<Take> takes = Players.TakeOneCardEach(Visibility.FaceUp);

                var numberOfPlaysStillInTheGame = takes.Count;

                TableViewsHistory.Add(takes.BuildView());

                Player playerOfHighestTake = takes.Max().Player;

                while (NeedBattle(takes.KeepTheLast(numberOfPlaysStillInTheGame), out var competitors))
                {
                    var faceDownTakes = competitors.TakeOneCardEach(Visibility.FaceDown);
                    TableViewsHistory.Add(faceDownTakes.BuildView());
                    takes.AddRange(faceDownTakes);

                    var faceUpTakes = competitors.TakeOneCardEach(Visibility.FaceUp);
                    if (!faceUpTakes.Any())
                    {
                        playerOfHighestTake = null;
                        DroppedCards.AddRange(takes.Select(x => x.Card));
                        if (this.Players.NobodyHasCards())
                        {
                            return Draw.Instance;
                        }

                        break;
                    }
                    
                    TableViewsHistory.Add(faceUpTakes.BuildView());
                    takes.AddRange(faceUpTakes);

                    playerOfHighestTake = faceUpTakes.Max().Player;

                    if (this.Players.OnlyOneStillHasCards(out var iAmTheOnlySurvivor))
                    {
                        playerOfHighestTake.Gather(takes);

                        return new HasWinner(iAmTheOnlySurvivor);
                    }

                    if (this.Players.NobodyHasCards())
                    {
                        DroppedCards.AddRange(takes.Select(x => x.Card));
                        return Draw.Instance;
                    }
                }

                playerOfHighestTake?.Gather(takes);
            }

            return new HasWinner(winner);
        }

        private static bool NeedBattle(IReadOnlyList<Take> levees, out List<Player> batailleCompetitors)
        {
            Take max = levees[0];
            batailleCompetitors = new List<Player>();

            foreach (var levee in levees)
            {
                var compareTo = levee.CompareTo(max);
                if (compareTo > 0)
                {
                    batailleCompetitors.Clear();

                    max = levee;
                    batailleCompetitors.Add(levee.Player);
                }
                else if (compareTo == 0)
                {
                    batailleCompetitors.Add(levee.Player);
                }
            }

            return batailleCompetitors.Count > 1;
        }
    }
}