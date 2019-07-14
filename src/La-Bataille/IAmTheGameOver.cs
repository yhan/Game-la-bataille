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
        public static IAmTheGameOver Instance = new Draw();
    }
}