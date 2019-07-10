using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Bataille
    {
        private readonly IShuffle _shuffle;
        public List<Joueur> Joueurs { get; private set; } = new List<Joueur>();

        public Bataille(int numberOfJoueurs, IShuffle shuffle)
        {
            _shuffle = shuffle;
            for (int i = 0; i < numberOfJoueurs; i++)
            {
                Joueurs.Add(new Joueur(i));
            }
        }

        public List<Vue> VuesPlateau { get; private set; } = new List<Vue>();

        public void Start()
        {
            Joueurs = _shuffle.DistributeCartes(Joueurs);

            var levees = Joueurs.Select(j => j.Lever()).ToArray();
            VuesPlateau.Add(new Vue(levees.Select(x => x.Carte)));

            var maxLevee = levees.Max();

            maxLevee.Joueur.Gagner(levees.Select(x => x.Carte));
        }
    }
}