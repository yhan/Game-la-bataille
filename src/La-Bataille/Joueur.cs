using System;
using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Joueur
    {
        public Joueur(int id)
        {
        }

        public Stack<Carte> Cartes { get; set; }

        public Levee Lever()
        {
            return new Levee(this, Cartes.Pop());
        }

        public void Gagner(IEnumerable<Carte> cartes)
        {
            var collection = cartes.Concat(cartes);
            Cartes = new Stack<Carte>(collection);
        }
    }

    public class Levee : IComparable<Levee>
    {
        public Joueur Joueur { get; }
        public Carte Carte { get; }

        public Levee(Joueur joueur, Carte carte)
        {
            Joueur = joueur;
            Carte = carte;
        }


        public int CompareTo(Levee other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return ((IComparable<Carte>) Carte).CompareTo(other.Carte);
        }
    }
}