namespace LaBataille
{
    /// <summary>
    /// Build a <see cref="Game"/>
    /// </summary>
    public class GameFactory
    {
        private readonly IDistributeCards _cardsDistributor;

        public GameFactory(IDistributeCards cardsDistributor)
        {
            _cardsDistributor = cardsDistributor;
        }

        public Game Build()
        {
            return new Game(_cardsDistributor);
        }
    }
}