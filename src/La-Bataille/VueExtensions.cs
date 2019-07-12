using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public static class VueExtensions
    {
        public static IEnumerable<Card> AsPureCartes(this View view)
        {
            return view.Select(x => x.Card);
        }
    }
}