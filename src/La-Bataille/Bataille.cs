using System.Collections.Generic;
using System.Linq;

namespace La_Bataille
{
    public class Bataille
    {
        private readonly IDistributeCartes _distributor;

        public Bataille(IDistributeCartes distributor)
        {
            _distributor = distributor;
            Joueurs = _distributor.DistributeCartes();
        }

        public List<Joueur> Joueurs { get; }

        public List<Vue> VuesPlateau { get; } = new List<Vue>();

        public bool JeuEnded(out Joueur vainqueur)
        {
            foreach (var joueur in Joueurs)
            {
                if (joueur.Paquet.Size == _distributor.TotalNumberOfCartes)
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
            while (Joueurs.All(j => j.Paquet.Size != _distributor.TotalNumberOfCartes))
            {
                var levees = Joueurs.LeverOneCarte(Visibilite.Devoilee);

                VuesPlateau.Add(BuildVue(levees));

                Levee maxLevee = levees.Max();

                while (NeedBataille(levees, out var competitors))
                {
                    var leveesCachees = competitors.LeverOneCarte(Visibilite.Cachee);
                    VuesPlateau.Add(BuildVue(leveesCachees));

                    var leveesDevoilee = competitors.LeverOneCarte(Visibilite.Devoilee);
                    VuesPlateau.Add(BuildVue(leveesDevoilee));

                    maxLevee = leveesDevoilee.Max();

                    levees.AddRange(leveesCachees);
                    levees.AddRange(leveesDevoilee);
                }

                maxLevee.Joueur.Gagner(levees.Select(x => x.Carte)
                                             .OrderByDescending(x => x) /*Put the smaller one on the bottom of Paquet, 
                                                                          to introduce some determinism for the following levee*/);
            }
        }

        private static bool NeedBataille(IReadOnlyCollection<Levee> levees, out List<Joueur> batailleCompetitors)
        {
            var maxLevee =  levees.Max();
            batailleCompetitors =  levees.Where(x => x.Carte == maxLevee.Carte).Select(l => l.Joueur).ToList();

            return batailleCompetitors.Count > 1;
        }

        private static Vue BuildVue(IEnumerable<Levee> levees)
        {
            return new Vue(levees.Select(l => new TwoFaceCarte(l.Carte, l.Visibilite)));
        }
    }
}