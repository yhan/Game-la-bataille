using System.Collections.Generic;

namespace La_Bataille
{
    /// <summary>
    ///  Routine which randomizes a collection of cartes
    /// 
    /// Implementation should have numberOfJoueurs as input
    /// </summary>
    public interface IDistributeCartes
    {
        List<Joueur> DistributeCartes();

        int TotalNumberOfCartes { get; set; }
    }
}