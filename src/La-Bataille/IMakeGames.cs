namespace LaBataille
{
    public interface IMakeGames
    {
        Game Build();
    }

    public class GameFactory: IMakeGames
    {
        private readonly IDistributeCards _cardsDistributor;

        public GameFactory( IDistributeCards cardsDistributor)
        {
            _cardsDistributor = cardsDistributor;
        }

        public Game Build()
        {
            return new Game(_cardsDistributor);
        }
    }
}