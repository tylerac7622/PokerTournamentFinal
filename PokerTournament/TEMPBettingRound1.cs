using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    //TEMP FILE SO WE CAN WORK ON THINGS AT THE SAME TIME
    class TEMPBettingRound1
    {
        //the ai handler for the first round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand, PlayerN player)
        {
            //hand[0] = new Card("Spades", 10);
            //hand[1] = new Card("Spades", 11);
            //hand[2] = new Card("Diamonds", 12);
            //hand[3] = new Card("Spades", 13);
            //hand[4] = new Card("Spades", 12);

            //list the hand, but only for debugging. EVENTUALLY don't show this
            AIEvaluate.ListTheHand(hand, player.Name);
            
            PlayerAction pa = null;
            ///Possible actions
            ///  bet - requires amount - cannot bet unless no previous bets in round (going first or only checks before)
            ///  raise - requires amount - cannot raise unless previous action was either bet or check (or fold for >2 players)
            ///  check - cannot check unless betting first or unless previous actions were checks
            ///  call - cannot call unless betting second and bet or raise was done previously
            ///  fold
            ///      Doing any action at the wrong time defaults to fold (player sacrifices their hand)
            ///      
            
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);
            int pRank = AIEvaluate.PotentialRank(hand);
            int currentBet = AIEvaluate.CurrentBet(actions);
            int willingBet = 0;
            int willingCheck = 0; //-1 for check on any bet greater than willingBet
            //manipulate the willing values for the ai based on their current cards
            if (rank == 1)
            {
                willingBet = 0;
                willingCheck = 0;
                willingBet += pRank * 5;
                willingCheck += pRank * 5;
            }
            else if (rank == 2)
            {
                willingBet = 10;
                willingCheck = 10;
                willingBet += pRank * 5;
                willingCheck += pRank * 5;
            }
            else if (rank == 3)
            {
                willingBet = 20;
                willingCheck = 30;
                willingBet += pRank * 5;
                willingCheck += pRank * 5;
            }
            else if (rank == 4)
            {
                willingBet = 20;
                willingCheck = 30;
                willingBet += pRank * 5;
                willingCheck += pRank * 5;
            }
            else if (rank == 5)
            {
                willingBet = 20;
                willingCheck = 30;
                willingBet += pRank * 5;
                willingCheck += pRank * 5;
            }
            else if (rank == 6)
            {
                willingBet = 0;
                willingCheck = 50;
            }
            else if(rank == 7 || rank == 8 || rank == 9 || rank == 10)//low bet, to try and draw the opponent in
            {
                willingBet = 10;
                willingCheck = -1;
                if (currentBet > 10)
                {
                    willingBet = 50;
                }
            }
            //chooses the next action based on what the ai is willing to do
            if (currentBet == 0)
            {
                if (willingBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "check", 0);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "bet", willingBet);
                }
            }
            else if (currentBet <= willingBet)
            {
                pa = new PlayerAction(player.Name, "Bet1", "raise", willingBet);
            }
            else if (currentBet <= willingCheck || willingCheck == -1)
            {
                pa = new PlayerAction(player.Name, "Bet1", "call", 0);
            }
            else
            {
                pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
            }
            return pa;
        }
    }
}
