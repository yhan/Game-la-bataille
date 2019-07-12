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


        public IAmTheGameOver Play(IShuffle shuffle)
        {
            while (Players.All(j => j.CardStack.Size != _distributor.TotalNumberOfCards))
            {
                List<Take> takes = Players.TakeOneCardEach(Visibility.FaceUp);

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

                    if (competitors.OnlyOneStillHasCards(out var winner))
                    {
                        playerOfHighestTake.Gather(takes.Select(x => x.Card)
                            .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);
                        return new HasWinner(winner);
                    }

                    if (competitors.NobodyHasCards())
                    {
                        return Draw.Instance;
                    }
                }

                playerOfHighestTake.Gather(takes.Select(x => x.Card)
                                             .OrderByDescending(x => x) /*Put the smaller one on the bottom of CardStack, 
                                                                          to introduce some determinism for the following levee*/);

                if (playerOfHighestTake.CardStack.Size == _distributor.TotalNumberOfCards)
                {
                    return new HasWinner(playerOfHighestTake); 
                }
            }

            return Draw.Instance;
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

    public class HasWinner : IAmTheGameOver
    {
        public Player Winner { get; }

        public HasWinner(Player winner)
        {
            Winner = winner;
        }
    }

    public class Draw : IAmTheGameOver
    {
        public static IAmTheGameOver Instance = new Draw();
    }

    public interface IAmTheGameOver
    {
    }
}