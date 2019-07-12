using System.Collections.Generic;
using System.Linq;
using Value;

namespace La_Bataille
{
    public class Joueur : ValueType<Joueur>
    {
        public int Id { get; }

        public Joueur(int id)
        {
            Id = id;
        }

        public Paquet Paquet { get; set; }

        public Levee Lever(Visibilite visibilite)
        {
            var popped = Paquet.Tirer();
            if (popped is NullCarte)
            {
                return null;
            }
            return new Levee(this, (Carte)popped, visibilite);
        }

        public void Gagner(IEnumerable<Carte> cartes)
        {
            foreach (var carte in cartes)
            {
                Paquet.MettreSousLePaquet(carte);
            }
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Id };
        }
    }

    public static class JoueurExtensions
    {
        public static List<Levee> LeverOneCarte(this IEnumerable<Joueur> joueurs,  Visibilite visibilite)
        {
            var levees = joueurs.Select(j => j.Lever(visibilite))
                .Where(l => l != null)
                .ToList();
            return levees;
        }
    }
}