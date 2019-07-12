using System.Collections.Generic;

namespace La_Bataille
{
    /// <summary>
    /// Implementation should have numberOfJoueurs as input
    /// </summary>
    public interface IShuffle
    {
        List<Joueur> DistributeCartes();

        int TotalNumberOfCartes { get; set; }
    }
}