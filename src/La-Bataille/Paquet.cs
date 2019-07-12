using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Paquet : IEnumerable<Carte>
    {
        private readonly List<Carte> _inner;

        public Paquet(IEnumerable<Carte> cartes)
        {
            _inner = cartes.ToList();
        }

        public IAmCarte Tirer()
        {
            if (_inner.Count == 0)
            {
                return NullCarte.Instance;
            }

            var last = _inner.Count - 1;
            var popped = _inner[last];
            _inner.RemoveAt(last);

            return popped;
        }

        public void MettreSousLePaquet(Carte carte)
        {
            _inner.Insert(0, carte);
        }

        public int Size => _inner.Count;

        public IEnumerator<Carte> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}