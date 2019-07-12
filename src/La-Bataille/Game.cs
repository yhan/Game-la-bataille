using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
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

        public bool IsGameOver(out Player winner)
        {
            foreach (var player in Players)
            {
                if (player.CardStack.Size == _distributor.TotalNumberOfCards)
                {
                    winner = player;
                    return true;
                }
            }

            winner = null;
            return false;
        }


        public void Start(IShuffle shuffle)
        {
            while (Players.All(j => j.CardStack.Size != _distributor.TotalNumberOfCards))
            {
                var takes = Players.TakeOneCardEach(Visibility.FaceUp);

                var numberOfPlaysStillInTheGame = takes.Count;

                TableViewsHistory.Add(takes.BuildView());

                Player playerOfHighestTake = takes.Max().Player;

                while (NeedBattle(takes.KeepTheLast(numberOfPlaysStillInTheGame), out var competitors))
                {
                    var faceDownTakes = competitors.TakeOneCardEach(Visibility.FaceDown);
                    TableViewsHistory.Add(faceDownTakes.BuildView());

                    var faceUpTakes = competitors.TakeOneCardEach(Visibility.FaceUp);
                    TableViewsHistory.Add(faceUpTakes.BuildView());

                    playerOfHighestTake = faceUpTakes.Max().Player;

                    takes.AddRange(faceDownTakes);
                    takes.AddRange(faceUpTakes);

                    if (competitors.OnlyOnePlayerStillHasCards(out var winner))
                    {
                        playerOfHighestTake = winner;
                        break;
                    }
                }

                playerOfHighestTake.Gather(takes.Select(x => x.Card)
                                             .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);
            }
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