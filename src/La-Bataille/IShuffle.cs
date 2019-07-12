namespace La_Bataille
{
    public interface IShuffle
    {
    }

    public class NullShuffle : IShuffle
    {
        public static IShuffle Instance = new NullShuffle();
    }
}