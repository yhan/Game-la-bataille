using System.Collections.Generic;
using System.Linq;
using La_Bataille;
using NSubstitute;

namespace Tests
{
    public class GameBuilder
    {
        /// <summary>
        /// Build a battle's initial state.
        /// </summary>
        /// <param name="distribution">Represent a stubbed distribution of cartes.
        ///     By Player, we have a list of CardStack. Hence a list of cartes list
        /// </param>
        /// <returns></returns>
        public static Game BuildGame(IList<List<Card>> distribution)
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