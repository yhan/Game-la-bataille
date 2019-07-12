using System;

namespace La_Bataille
{
    public static class CarteExtensions
    {
        public static Carte AsTrefles(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Trefle);
        }

        public static Carte AsCarreaux(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Carreaux);
        }


        public static Carte AsCoeur(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Coeur);
        }


        public static Carte AsPique(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Carte value should be inclusively between 2 and 14");
            }
            return new Carte(value, Figure.Pique);
        }

        public static TwoFaceCarte AsDevoilee(this Carte carte)
        {
            return new TwoFaceCarte(carte, Visibilite.Devoilee);
        }

        
        public static TwoFaceCarte AsCachee(this Carte carte)
        {
            return new TwoFaceCarte(carte, Visibilite.Cachee);
        }
    }
}