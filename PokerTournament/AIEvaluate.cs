using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class AIEvaluate
    {
        //list of helper functions telling how close a hand is to certain combos
        public static int CloseToRoyalFlush(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                //royal flush must start at 10
                if (hand[i].Value > 9)
                {
                    currentTracker = hand[i].Value;
                    currentLength = 1;
                    //check each card after to see if it could be a part of a straight including the first card
                    for (int i2 = i + 1; i2 < 5; i2++)
                    {
                        //another card is a part of the straight flush if it is within 5 value of the start and isn't a repeat value of the previous card in the straight and the suit is the same as the first card
                        if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                        {
                            currentLength++;
                            currentTracker = hand[i2].Value;
                        }
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }
            return maxLength;
        }
        public static int CloseToStraightFlush(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                currentTracker = hand[i].Value;
                currentLength = 1;
                //check each card after to see if it could be a part of a straight including the first card
                for (int i2 = i + 1; i2 < 5; i2++)
                {
                    //another card is a part of the straight flush if it is within 5 value of the start and isn't a repeat value of the previous card in the straight and the suit is the same as the first card
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                    {
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }
            return maxLength;
        }
        public static int CloseToStraight(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                currentTracker = hand[i].Value;
                currentLength = 1;
                //check each card after to see if it could be a part of a straight including the first card
                for (int i2 = i + 1; i2 < 5; i2++)
                {
                    //another card is a part of the straight if it is within 5 value of the start and isn't a repeat value of the previous card in the straight
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker)
                    {
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                }
            }
            return maxLength;
        }
        public static int CloseToFlush(Card[] hand)
        {
            // sort the hand
            Evaluate.SortHand(hand);

            int maxLength = 1;

            //returns the largest count of each suit, since that suit will be closest to a flush
            int diamonds = AllSameSuit(hand, "Diamonds");
            int spades = AllSameSuit(hand, "Spades");
            int clubs = AllSameSuit(hand, "Clubs");
            int hearts = AllSameSuit(hand, "Hearts");

            maxLength = Math.Max(Math.Max(diamonds, hearts), Math.Max(spades, clubs));

            return maxLength;
        }
        //returns the greatest "-of-a-kind" combo currently in the hand
        public static int MostOfSameValue(Card[] hand)
        {
            int mostCount = 1;
            for (int i = 2; i < 15; i++)
            {
                if (Evaluate.ValueCount(i, hand) > mostCount)
                {
                    mostCount = Evaluate.ValueCount(i, hand);
                }
            }
            return mostCount;
        }
        public static int CloseToFullHouse(Card[] hand)
        {
            int chosen = 0;
            int firstPairingCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1)
                {
                    chosen = hand[i].Value;
                    firstPairingCount = Evaluate.ValueCount(i, hand);
                }
            }
            if (firstPairingCount == 0)
            {
                return 2;
            }
            int secondPairingCount = 1;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1 && hand[i].Value != chosen)
                {
                    secondPairingCount = Evaluate.ValueCount(i, hand);
                }
            }
            return firstPairingCount + secondPairingCount;
        }
        //like the fullhouse one, but only checking for 2-2 (and thus locking 3 of a kind to 2)
        public static int CloseToTwoPair(Card[] hand)
        {
            int chosen = 0;
            int firstPairingCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1)
                {
                    chosen = hand[i].Value;
                    firstPairingCount = 2;
                }
            }
            if (firstPairingCount == 0)
            {
                return 2;
            }
            int secondPairingCount = 1;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1 && hand[i].Value != chosen)
                {
                    secondPairingCount = 2;
                }
            }
            return firstPairingCount + secondPairingCount;
        }

        //helper function returning the number of cards in a hand that are of the passed in suit
        public static int AllSameSuit(Card[] hand, string suit)
        {
            int returnCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (hand[i].Suit == suit)
                {
                    returnCount++;
                }
            }
            return returnCount;
        }

        //This function returns the rank of potential hand based on how close the current hand is to combos
        public static int PotentialRank(Card[] hand)
        {
            //how much is needed to be close to a full hand combo
            int needForFive = 3;

            if(CloseToRoyalFlush(hand) >= needForFive)
            {
                return 10;
            }
            if (CloseToStraightFlush(hand) >= needForFive)
            {
                return 9;
            }
            if (MostOfSameValue(hand) >= 3)
            {
                return 8;
            }
            if (CloseToFullHouse(hand) >= 4)
            {
                return 7;
            }
            if (CloseToFlush(hand) >= needForFive)
            {
                return 6;
            }
            if (CloseToStraight(hand) >= needForFive)
            {
                return 5;
            }
            if (MostOfSameValue(hand) == 2)
            {
                return 4;
            }
            return 2; //one pair is the worst potential hand that is returned
        }

        public static void ListTheHand(Card[] hand, String name)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.Write("\nName: " + name + "\n\tRank: " + PrintRank(rank) + "\n\tPotential Rank: " + PrintRank(PotentialRank(hand)) + "\n\tTheir hand:");
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write("\n\t " + hand[i].ToString() + " ");
            }
            Console.WriteLine();
            Console.WriteLine("\n\t " + CloseToRoyalFlush(hand));
        }

        //returns the string version of a rank, for prettier outputs
        public static String PrintRank(int rank)
        {
            switch (rank)
            {
                case 2:
                    {
                        return "One Pair";
                    }
                case 3:
                    {
                        return "Two Pair";
                    }
                case 4:
                    {
                        return "Three of a Kind";
                    }
                case 5:
                    {
                        return "Straight";
                    }
                case 6:
                    {
                        return "Flush";
                    }
                case 7:
                    {
                        return "Full House";
                    }
                case 8:
                    {
                        return "Four of a Kind";
                    }
                case 9:
                    {
                        return "Straight Flush";
                    }
                case 10:
                    {
                        return "Royal Flush";
                    }
            }
            return "High Card";
        }

        //returns the last action by a player, or null if there is no previous action
        public static PlayerAction LastAction(List<PlayerAction> actions)
        {
            if(actions.Count > 0)
            {
                return actions[actions.Count - 1];
            }
            else
            {
                return null;
            }
        }

        //returns the current bet requirement, or 0 if no one has bet yet
        public static int CurrentBet(List<PlayerAction> actions)
        {
            int actionIter = actions.Count - 1;
            while(actionIter > 0)
            {
                if(actions[actionIter].ActionName == "Bet" || actions[actionIter].ActionName == "Raise")
                {
                    return actions[actionIter].Amount;
                }
            }
            return 0;
        }
    }
}
