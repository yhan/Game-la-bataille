using System.Collections.Generic;
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
            return new Levee(this, popped, visibilite);
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
}