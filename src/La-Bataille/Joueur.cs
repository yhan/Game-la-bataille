using System;
using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Joueur
    {
        public int Id { get; }

        public Joueur(int id)
        {
            Id = id;
        }

        public IList<Carte> Cartes { get; set; }

        public Levee Lever()
        {
            var last = Cartes.Count - 1;
            var popped = Cartes[last];
            Cartes.RemoveAt(last);
            return new Levee(this, popped);
        }

        public void Gagner(IEnumerable<Carte> cartes)
        {
            foreach (var carte in cartes)
            {
                Cartes.Insert(0, carte);    
            }
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