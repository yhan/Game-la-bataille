using System.Collections.Generic;

namespace La_Bataille
{
    /// <summary>
    ///  Routine which randomizes a collection of cartes
    /// 
    /// Implementation should have numberOfJoueurs as input
    /// </summary>
    public interface IDistributeCards
    {
        List<Player> Distribute();

        int TotalNumberOfCards { get; set; }
    }
}