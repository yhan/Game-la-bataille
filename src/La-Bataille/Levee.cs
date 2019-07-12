using System;

namespace La_Bataille
{
    public class Levee : IComparable<Levee>
    {
        public Joueur Joueur { get; }
        public Carte Carte { get; }
        public Visibilite Visibilite { get; set; }

        public Levee(Joueur joueur, Carte carte, Visibilite visibilite)
        {
            Joueur = joueur;
            Carte = carte;
            Visibilite = visibilite;
        }


        public int CompareTo(Levee other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return ((IComparable<Carte>)Carte).CompareTo(other.Carte);
        }
    }
}