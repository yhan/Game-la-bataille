using System.Collections.Generic;
using System.Linq;

namespace LaBataille
{

    /// <summary>
    /// Generates all <see cref="Card"/>s
    /// </summary>
    public class CardsProvider
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