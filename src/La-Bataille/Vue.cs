using System.Collections;
using System.Collections.Generic;

namespace La_Bataille
{
    public class Vue : IEnumerable<TwoFaceCarte>
    {
        private readonly IEnumerable<TwoFaceCarte> _cartes;

        public Vue(IEnumerable<TwoFaceCarte> cartes)
        {
            _cartes = cartes;
        }

        public IEnumerator<TwoFaceCarte> GetEnumerator()
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

    public struct TwoFaceCarte
    {
        public Carte Carte { get; }
        public Visibilite Visibilite { get; }

        public TwoFaceCarte(Carte carte, Visibilite visibilite)
        {
            Carte = carte;
            Visibilite = visibilite;
        }

        public override string ToString()
        {
            return $"{Carte}({Visibilite})";
        }
    }

    public enum Visibilite
    {
        Devoilee,
        Cachee
    }
}