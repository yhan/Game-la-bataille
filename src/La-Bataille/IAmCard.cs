namespace LaBataille
{
    public interface IAmCard
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

    
    public class NullCard : IAmCard
    {
        public int Value { get; } = 0;
        public Figure Figure { get; } = Figure.Null;
        public static NullCard Instance = new NullCard();
    }
}