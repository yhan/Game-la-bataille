using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Bataille
    {
        private readonly IShuffle _shuffle;

        public Bataille(IShuffle shuffle)
        {
            _shuffle = shuffle;
            Joueurs = _shuffle.DistributeCartes();
        }

        public List<Joueur> Joueurs { get; }

        public List<Vue> VuesPlateau { get; } = new List<Vue>();

        public bool JeuEnded(out Joueur vainqueur)
        {
            foreach (var joueur in Joueurs)
            {
                if (joueur.Cartes.Count == _shuffle.TotalNumberOfCartes)
                {
                    vainqueur = joueur;
                    return true;
                }
            }

            vainqueur = null;
            return false;
        }


        public void Start()
        {
            var levees = Joueurs.Select(j => j.Lever()).ToArray();
            VuesPlateau.Add(new Vue(levees.Select(x => x.Carte)));

            var maxLevee = levees.Max();

            maxLevee.Joueur.Gagner(levees.Select(x => x.Carte));
        }
    }
}