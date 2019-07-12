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
                if (joueur.Paquet.Size == _shuffle.TotalNumberOfCartes)
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
            while (Joueurs.All(j => j.Paquet.Size != 0))
            {
                var levees = Joueurs.Select(j => j.Lever()).ToArray();
                VuesPlateau.Add(new Vue(levees.Select(x => x.Carte)));

                var maxLevee = levees.Max();

                maxLevee.Joueur.Gagner(levees.Select(x => x.Carte).OrderByDescending(x => x)/*Put the smaller one on the bottom of Paquet, to introduce some determinism for the following levee*/);     
            }
        }
    }
}