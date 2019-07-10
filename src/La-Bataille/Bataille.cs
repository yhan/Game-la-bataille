using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Bataille
    {
        private readonly IShuffle _shuffle;
        public readonly List<Joueur> Joueurs = new List<Joueur>();

        public Bataille(int numberOfJoueurs, IShuffle shuffle)
        {
            _shuffle = shuffle;
            for (int i = 0; i < numberOfJoueurs; i++)
            {
                Joueurs.Add(new Joueur(i));
            }
        }

        public List<Vue> VuesPlateau { get; set; }

        public void Start()
        {
            _shuffle.DistributeCartes(Joueurs);

            var cartesToCompare = Joueurs.Select(j => j.Lever());
            var gagantLevee = cartesToCompare.Max();

            gagantLevee.Joueur.Gagner(cartesToCompare.Select(x => x.Carte));

        }
    }
}