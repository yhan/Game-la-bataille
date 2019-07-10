using System.Collections.Generic;

namespace La_Bataille
{
    public interface IShuffle
    {
        List<Joueur> DistributeCartes(List<Joueur> joueurs);
    }
}