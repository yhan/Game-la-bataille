using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public interface IProvideCards
    {
        IEnumerable<Card> Provide();
    }

    public class CardsProvider : IProvideCards
    {
        public static  CardsProvider Instance = new CardsProvider();

        public IEnumerable<Card> Provide()
        {
            return Card.ValidValuesRange
                .Select(x => new Card[] { x.AsHeart(), x.AsClub(), x.AsDiamond(), x.AsSpade() })
                .SelectMany(x => x);
        }
    }
}