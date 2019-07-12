using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Bataille
    {
        private readonly IDistributeCartes _distributeCartes;

        public Bataille(IDistributeCartes distributeCartes)
        {
            _distributeCartes = distributeCartes;
            Joueurs = _distributeCartes.DistributeCartes();
        }

        public List<Joueur> Joueurs { get; }

        public List<Vue> VuesPlateau { get; } = new List<Vue>();

        public bool JeuEnded(out Joueur vainqueur)
        {
            foreach (var joueur in Joueurs)
            {
                if (joueur.Paquet.Size == _distributeCartes.TotalNumberOfCartes)
                {
                    vainqueur = joueur;
                    return true;
                }
            }

            vainqueur = null;
            return false;
        }


        public void Start(IShuffle shuffle)
        {
            while (Joueurs.All(j => j.Paquet.Size != 0))
            {
                var levees = Joueurs.Select(j => j.Lever(Visibilite.Devoilee)).ToArray();
                VuesPlateau.Add(new Vue(levees.Select( l => new TwoFaceCarte(l.Carte, l.Visibilite))));

                var maxLevee = levees.Max();

                maxLevee.Joueur.Gagner(levees.Select(x => x.Carte).OrderByDescending(x => x)/*Put the smaller one on the bottom of Paquet, to introduce some determinism for the following levee*/);     
            }
        }
    }


    public interface IShuffle
    {
    }

    public class NullShuffle : IShuffle
    {
        public static IShuffle Instance = new NullShuffle();
    }
}