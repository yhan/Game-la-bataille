namespace La_Bataille
{
    public interface IAmCarte
    {
        /// <summary>
        /// 2, 3, ..., 10, 11(valet,), 12(dame), 13(roi), 14(as)
        /// </summary>
        int Value { get; }

        /// <summary>
        /// clubs/trefles (♣), diamonds/carreaux (♦), hearts/coeurs (♥) and spades/pique (♠)
        /// </summary>
        Figure Figure { get; }
    }

    
    public class NullCarte : IAmCarte
    {
        public int Value { get; } = 0;
        public Figure Figure { get; } = Figure.Null;
        public static NullCarte Instance = new NullCarte();
    }
}