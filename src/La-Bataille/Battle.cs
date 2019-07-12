using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Battle
    {
        private readonly IDistributeCards _distributor;

        public Battle(IDistributeCards distributor)
        {
            _distributor = distributor;
            Players = _distributor.Distribute();
        }

        public List<Player> Players { get; }

        public List<View> TableViewsHistory { get; } = new List<View>();

        public bool IsGameOver(out Player vainqueur)
        {
            foreach (var player in Players)
            {
                if (player.CardStack.Size == _distributor.TotalNumberOfCards)
                {
                    vainqueur = player;
                    return true;
                }
            }

            vainqueur = null;
            return false;
        }


        public void Start(IShuffle shuffle)
        {
            while (Players.All(j => j.CardStack.Size != _distributor.TotalNumberOfCards))
            {
                var takes = Players.TakeOneCardEach(Visibility.FaceUp);

                var numberOfPlaysStillInTheGame = takes.Count;

                TableViewsHistory.Add(BuildView(takes));

                Take highestTake = takes.Max();

                while (NeedBattle(TakeLast(takes, numberOfPlaysStillInTheGame), out var competitors))
                {
                    var faceDownTakes = competitors.TakeOneCardEach(Visibility.FaceDown);
                    TableViewsHistory.Add(BuildView(faceDownTakes));

                    var faceUpTakes = competitors.TakeOneCardEach(Visibility.FaceUp);
                    TableViewsHistory.Add(BuildView(faceUpTakes));

                    highestTake = faceUpTakes.Max();

                    takes.AddRange(faceDownTakes);
                    takes.AddRange(faceUpTakes);
                }

                highestTake.Player.Gather(takes.Select(x => x.Card)
                                             .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);
            }
        }

        private List<Take> TakeLast(IEnumerable<Take> levees, int numberOfJoueursInTheGame)
        {
            return levees.Reverse().Take(numberOfJoueursInTheGame).ToList();
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

        private static View BuildView(IEnumerable<Take> levees)
        {
            return new View(levees.Select(l => new TwoFaceCard(l.Card, l.Visibility)));
        }
    }
}