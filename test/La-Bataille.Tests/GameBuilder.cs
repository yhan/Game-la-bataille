using System.Collections.Generic;
using System.Linq;
using NSubstitute;

namespace LaBataille.Tests
{
    public static class GameBuilder
    {
        /// <summary>
        /// Build a battle's initial state.
        /// </summary>
        /// <param name="distribution">Represent a stubbed distribution of cards.
        ///     By Player, we have a list of CardStack(a list of cards list).
        /// </param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Game BuildGame(IList<List<Card>> distribution, IEnumerable<Player> source)
        {
            var numberOfJoueurs = distribution.Count;

            var playersCollection = source.ToArray();

            for (int i = 0; i < numberOfJoueurs; i++)
            {
                playersCollection[i].CardStack = new CardStack(distribution[i]);
            }

            IDistributeCards cardsDistributor = Substitute.For<IDistributeCards>();
            cardsDistributor.DistributedCardsSize.Returns(distribution.SelectMany(x => x).Count());
            cardsDistributor.Distribute().Returns(playersCollection.ToList());

            var game = new Game(cardsDistributor);

            return game;
        }
    }
}