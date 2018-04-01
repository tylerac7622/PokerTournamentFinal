using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class AIEvaluate
    {
        //list of helper functions that returns a 2d list of current card combinations that are close to the checked combo
        public static List<List<Card>> CloseToRoyalFlushCards(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            List<List<Card>> cardCombos = new List<List<Card>>();
            List<Card> currentList = new List<Card>();

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                //royal flush must start at 10
                if (hand[i].Value > 9)
                {
                    currentList.Add(hand[i]);
                    currentTracker = hand[i].Value;
                    currentLength = 1;
                    //check each card after to see if it could be a part of a straight including the first card
                    for (int i2 = i + 1; i2 < 5; i2++)
                    {
                        //another card is a part of the straight flush if it is within 5 value of the start and isn't a repeat value of the previous card in the straight and the suit is the same as the first card
                        if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                        {
                            currentList.Add(hand[i2]);
                            currentLength++;
                            currentTracker = hand[i2].Value;
                        }
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    //also, since there is a new longest length, remove all previous saved potential combos and restart the 2d list with the current one
                    cardCombos.Clear();
                    cardCombos.Add(new List<Card>(currentList));
                }
                //if it's the same length, it's a new iteration in the returned list
                else if(currentLength == maxLength && currentLength != 0)
                {
                    cardCombos.Add(new List<Card>(currentList));
                }
                currentList.Clear();
            }
            return cardCombos;
        }
        public static List<List<Card>> CloseToStraightFlushCards(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            List<List<Card>> cardCombos = new List<List<Card>>();
            List<Card> currentList = new List<Card>();

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                currentList.Add(hand[i]);
                currentTracker = hand[i].Value;
                currentLength = 1;
                //check each card after to see if it could be a part of a straight including the first card
                for (int i2 = i + 1; i2 < 5; i2++)
                {
                    //another card is a part of the straight flush if it is within 5 value of the start and isn't a repeat value of the previous card in the straight and the suit is the same as the first card
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker && hand[i2].Suit == hand[i].Suit)
                    {
                        currentList.Add(hand[i2]);
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    //also, since there is a new longest length, remove all previous saved potential combos and restart the 2d list with the current one
                    cardCombos.Clear();
                    cardCombos.Add(new List<Card>(currentList));
                }
                //if it's the same length, it's a new iteration in the returned list
                else if (currentLength == maxLength && currentLength != 0)
                {
                    cardCombos.Add(new List<Card>(currentList));
                }
                currentList.Clear();
            }
            return cardCombos;
        }
        public static List<List<Card>> CloseToStraightCards(Card[] hand)
        {
            // sort the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            List<List<Card>> cardCombos = new List<List<Card>>();
            List<Card> currentList = new List<Card>();

            int maxLength = 0;
            int currentLength = 0;
            int currentTracker = hand[0].Value;
            //loops through each card, checking if it is the earliest point of a possible straight
            for (int i = 0; i < 5; i++)
            {
                currentList.Add(hand[i]);
                currentTracker = hand[i].Value;
                currentLength = 1;
                //check each card after to see if it could be a part of a straight including the first card
                for (int i2 = i + 1; i2 < 5; i2++)
                {
                    //another card is a part of the straight if it is within 5 value of the start and isn't a repeat value of the previous card in the straight
                    if (hand[i2].Value - hand[i].Value < 5 && hand[i2].Value != currentTracker)
                    {
                        currentList.Add(hand[i2]);
                        currentLength++;
                        currentTracker = hand[i2].Value;
                    }
                }
                //the largest potential straight that can be found is returned
                if (currentLength > maxLength)
                {
                    maxLength = currentLength;
                    //also, since there is a new longest length, remove all previous saved potential combos and restart the 2d list with the current one
                    cardCombos.Clear();
                    cardCombos.Add(new List<Card>(currentList));
                }
                //if it's the same length, it's a new iteration in the returned list
                else if (currentLength == maxLength && currentLength != 0)
                {
                    cardCombos.Add(new List<Card>(currentList));
                }
                currentList.Clear();
            }
            return cardCombos;
        }
        public static List<List<Card>> CloseToFlushCards(Card[] hand)
        {
            // sort the hand
            Evaluate.SortHand(hand);

            int maxLength = 1;

            List<List<Card>> cardCombos = new List<List<Card>>();

            //returns the largest count of each suit, since that suit will be closest to a flush
            int diamonds = AllSameSuit(hand, "Diamonds");
            int spades = AllSameSuit(hand, "Spades");
            int clubs = AllSameSuit(hand, "Clubs");
            int hearts = AllSameSuit(hand, "Hearts");

            maxLength = Math.Max(Math.Max(diamonds, hearts), Math.Max(spades, clubs));

            //adds the list of cards which are close to a flush to the return list
            List<Card> diamondsCards = AllSameSuitCards(hand, "Diamonds");
            List<Card> spadesCards = AllSameSuitCards(hand, "Spades");
            List<Card> clubsCards = AllSameSuitCards(hand, "Clubs");
            List<Card> heartsCards = AllSameSuitCards(hand, "Hearts");

            if(diamondsCards.Count == maxLength)
            {
                cardCombos.Add(new List<Card>(diamondsCards));
            }
            if (spadesCards.Count == maxLength)
            {
                cardCombos.Add(new List<Card>(spadesCards));
            }
            if (clubsCards.Count == maxLength)
            {
                cardCombos.Add(new List<Card>(clubsCards));
            }
            if (heartsCards.Count == maxLength)
            {
                cardCombos.Add(new List<Card>(heartsCards));
            }

            return cardCombos;
        }
        public static List<List<Card>> MostOfSameValueCards(Card[] hand)
        {
            int mostCount = 1;
            for (int i = 2; i < 15; i++)
            {
                if (Evaluate.ValueCount(i, hand) > mostCount)
                {
                    mostCount = Evaluate.ValueCount(i, hand);
                }
            }
            List<List<Card>> cardCombos = new List<List<Card>>();
            for (int i = 2; i < 15; i++)
            {
                List<Card> combo = ValueCountCards(i, hand);
                if (combo.Count == mostCount)
                {
                    cardCombos.Add(new List<Card>(combo));
                }
            }
            return cardCombos;
        }

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
        public static List<Card> AllSameSuitCards(Card[] hand, string suit)
        {
            List<Card> returnCards = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                if (hand[i].Suit == suit)
                {
                    returnCards.Add(hand[i]);
                }
            }
            return returnCards;
        }
        //helper function returning all cards with the specified value
        public static List<Card> ValueCountCards(int value, Card[] hand)
        {
            // count the occurences of a value
            List<Card> countCards = new List<Card>();
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Value == value)
                {
                    countCards.Add(hand[i]);
                }
            }
            return countCards;
        }

        //This function returns the rank of potential hand based on how close the current hand is to combos
        public static int PotentialRank(Card[] hand)
        {
            //how much is needed to be close to a full hand combo
            int needForFive = 3;

            if(CloseToRoyalFlush(hand) >= needForFive)
            {
                //at least 60% of combo is close
                return 10;
            }
            if (CloseToStraightFlush(hand) >= needForFive)
            {
                //at least 60% of combo is close
                return 9;
            }
            if (MostOfSameValue(hand) >= 3) //will have at least 3 of a kind (rank = 4)
            {
                //at least 75% of combo is close
                return 8;
            }
            if (CloseToFullHouse(hand) >= 4) //will have at least 2 pair (rank = 3)
            {
                //at least 80% of combo is close
                return 7;
            }
            if (CloseToFlush(hand) >= needForFive)
            {
                //at least 60% of combo is close
                return 6;
            }
            if (CloseToStraight(hand) >= needForFive)
            {
                //at least 60% of combo is close
                return 5;
            }
            if (MostOfSameValue(hand) == 2) //will have at least 1 pair (rank = 2)
            {
                //at least 66% of combo is close
                return 4;
            }
            //two pair is not included, since close to 2 pair is also close to 3 of a kind
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
            /*List<List<Card>> close = CloseToRoyalFlushCards(hand);
            for (int i = 0; i < close.Count; i++)
            {
                for (int i2 = 0; i2 < close[i].Count; i2++)
                {
                    Console.Write("\n\t\t " + close[i][i2].ToString() + " ");
                }
                Console.Write("\n\t\t------------");
            }
            Console.WriteLine("\n\t " + CloseToRoyalFlush(hand));*/
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
            //int actionIter = 0;
            int betTotal = 0;

            //while (actionIter < actions.Count)
            while (actionIter >= 0)
            {
                if (actions[actionIter].ActionName == "bet")
                {
                    return actions[actionIter].Amount + betTotal;
                }
                if (actions[actionIter].ActionName == "raise")
                {
                    betTotal += actions[actionIter].Amount;
                }
                //actionIter++;
                actionIter--;
            }
            return 0;
        }
    }
}
