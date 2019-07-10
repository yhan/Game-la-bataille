using System.Collections.Generic;

namespace La_Bataille
{
    public class Vue
    {
        public IEnumerable<Carte> Cartes { get; set; }

        public Vue(IEnumerable<Carte> cartes)
        {
            Cartes = cartes;
        }

        public override string ToString()
        {
            return string.Join(", ", Cartes);
        }
    }
}