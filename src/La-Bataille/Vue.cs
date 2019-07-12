using System.Collections;
using System.Collections.Generic;

namespace La_Bataille
{
    public class Vue : IEnumerable<Carte>
    {
        private readonly IEnumerable<Carte> _cartes;

        public Vue(IEnumerable<Carte> cartes)
        {
            _cartes = cartes;
        }

        public IEnumerator<Carte> GetEnumerator()
        {
            return _cartes.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", _cartes);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}