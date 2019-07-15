namespace LaBataille
{
    public interface IAmTheGameOver
    {
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
        public string Reason { get; }
        public static IAmTheGameOver Instance = new Draw(string.Empty);

        public Draw(string reason)
        {
            Reason = reason;
        }
    }

    public class NullDraw  : IAmTheGameOver
    {
        public string Reason { get; }

        public NullDraw(string reason)
        {
            Reason = reason;
        }
    }
}