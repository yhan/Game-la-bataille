using System;

namespace La_Bataille
{
    public static class CardExtensions
    {
        public static Card AsClub(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value should be inclusively between 2 and 14");
            }
            return new Card(value, Figure.Club);
        }

        public static Card AsDiamond(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value should be inclusively between 2 and 14");
            }
            return new Card(value, Figure.Diamond);
        }


        public static Card AsHeart(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value should be inclusively between 2 and 14");
            }
            return new Card(value, Figure.Heart);
        }


        public static Card AsSpade(this int value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value should be inclusively between 2 and 14");
            }
            return new Card(value, Figure.Spade);
        }

        public static TwoFaceCard FaceUp(this Card card)
        {
            return new TwoFaceCard(card, Visibility.FaceUp);
        }

        
        public static TwoFaceCard FaceDown(this Card card)
        {
            return new TwoFaceCard(card, Visibility.FaceDown);
        }
    }
}