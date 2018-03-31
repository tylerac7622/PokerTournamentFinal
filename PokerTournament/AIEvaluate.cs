using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class AIEvaluate
    {
        public static int CloseToRoyalFlush(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            for (int i = 0; i < 5; i++)
            {
                if (hand[i].Value > 9)
                {
                    currentTracker = hand[i].Value;
                    currentLength = 1;
                    for (int i2 = i + 1; i2 < 5; i2++)
                    {
                        if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                        {
                            currentLength++;
                            currentTracker = hand[i2].Value;
                        }
                    }
                }
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

            int maxLength = 1;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            for (int i = 0; i < 5; i++)
            {
                currentTracker = hand[i].Value;
                currentLength = 1;
                for (int i2 = i + 1; i2 < 5; i2++)
                {
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                    {
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
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

            int maxLength = 1;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            for (int i = 0; i < 5; i++)
            {
                currentTracker = hand[i].Value;
                currentLength = 1;
                for (int i2 = i+1; i2 < 5; i2++)
                {
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker)
                    {
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
                if(currentLength > maxLength)
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

            int diamonds = AllSameSuit(hand, "Diamonds");
            int spades = AllSameSuit(hand, "Spades");
            int clubs = AllSameSuit(hand, "Clubs");
            int hearts = AllSameSuit(hand, "Hearts");

            maxLength = Math.Max(Math.Max(diamonds, hearts), Math.Max(spades, clubs));

            return maxLength;
        }

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

        public static int CloseToFullHouse(Card[] hand)
        {
            int chosen = 0;
            int firstPairingCount = 0;
            for(int i = 0; i < 5; i++)
            {
                if(Evaluate.ValueCount(i, hand) > 1)
                {
                    chosen = hand[i].Value;
                    firstPairingCount = Evaluate.ValueCount(i, hand);
                }
            }
            if(firstPairingCount == 0)
            {
                return 0;
            }
            int secondPairingCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1 && hand[i].Value != chosen)
                {
                    secondPairingCount = Evaluate.ValueCount(i, hand);
                }
            }
            return firstPairingCount + secondPairingCount;
        }
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
                return 0;
            }
            int secondPairingCount = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Evaluate.ValueCount(i, hand) > 1 && hand[i].Value != chosen)
                {
                    secondPairingCount = 2;
                }
            }
            return firstPairingCount + secondPairingCount;
        }

        public static void ListTheHand(Card[] hand, String name)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.Write("\nName: " + name + "\n\tRank: " + PrintRank(rank) + "\n\tTheir hand:");
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write("\n\t " + hand[i].ToString() + " ");
            }
            Console.WriteLine();
            Console.WriteLine("\n\t " + CloseToRoyalFlush(hand));
        }

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
