using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public static class VueExtensions
    {
        public static IEnumerable<Carte> AsPureCartes(this Vue vue)
        {
            return vue.Select(x => x.Carte);
        }
    }
}