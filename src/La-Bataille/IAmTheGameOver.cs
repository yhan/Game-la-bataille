namespace LaBataille
{
    /// <summary>
    /// Represent the end of a <see cref="Game"/>
    /// </summary>
    public interface IAmTheGameOver
    {
    }

    /// <summary>
    /// Game finds a winner
    /// </summary>
    public class HasWinner : IAmTheGameOver
    {
        public Player Winner { get; }

        public HasWinner(Player winner)
        {
            Winner = winner;
        }
    }

    /// <summary>
    /// Game is a draw
    /// </summary>
    public class Draw : IAmTheGameOver
    {
        public string Reason { get; }
        public static IAmTheGameOver Instance = new Draw(string.Empty);

        public Draw(string reason)
        {
            Reason = reason;
        }
    }

    /// <summary>
    /// Technical. Game is not ended.
    /// Useful when we are in iteration, mark game is still in a not ended state.
    /// </summary>
    public class GameOngoing  : IAmTheGameOver
    {
        public string Reason { get; }

        public GameOngoing(string reason)
        {
            Reason = reason;
        }
    }
}