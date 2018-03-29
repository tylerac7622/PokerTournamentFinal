using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class AIEvaluate
    {
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
