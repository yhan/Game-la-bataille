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

        public Levee Lever()
        {
            var popped = Paquet.Pop();
            return new Levee(this, popped);
        }

        public void Gagner(IEnumerable<Carte> cartes)
        {
            foreach (var carte in cartes)
            {
                Paquet.PutOnTheHead(carte);
            }
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Id };
        }
    }
}