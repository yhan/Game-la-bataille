using System;
using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{
    /// <summary>
    ///  Routine which randomizes a collection of cartes
    /// 
    /// Implementation should have numberOfJoueurs (and maybe all the cards to play with) as input
    /// </summary>
    public interface IDistributeCards
    {
        List<Player> Distribute();

        int DistributedCardsSize { get; set; }
    }

    public class CardsDistributor : IDistributeCards
    {
        private readonly int _numberOfPlayers;
        private IEnumerable<Card> _cards;
        private readonly Player[] _players;
        private readonly int _stackSize;

        public CardsDistributor(CardsProvider cardsProvider, IEnumerable<Player> players)
        {
            _players = players.ToArray();
            if (_players.Length > 17)
            {
                throw new ArgumentException("Each player should have at least 3 cards. Number of players can not exceed 17. ");
            }

            _numberOfPlayers = _players.Length;
            _cards = cardsProvider.Provide();

            _stackSize = _cards.Count() / _numberOfPlayers;
            DistributedCardsSize = _players.Length * _stackSize;
        }

        public List<Player> Distribute()
        {
            var cards = _cards.ToList();
            _cards = cards.Shuffle();

            for (int i = 0; i < _numberOfPlayers; i++)
            {
                _players[i].CardStack = new CardStack(_cards.Skip(i*_stackSize).Take(_stackSize));
            }

            return _players.ToList();
        }

        public int DistributedCardsSize { get; set; }
    }
}