namespace LaBataille
{
    /// <summary>
    /// Can inspect a <see cref="Game"/> and the game over result: <see cref="IAmTheGameOver"/>.
    /// Not supposed to do any mutation on them.
    /// </summary>
    public interface IVisitGameOver
    {
        void Visit(Game game, IAmTheGameOver gameOver);
    }
}