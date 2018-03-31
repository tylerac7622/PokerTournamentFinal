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
            hand[0] = new Card("Spades", 10);
            hand[1] = new Card("Spades", 11);
            hand[2] = new Card("Diamonds", 12);
            hand[3] = new Card("Spades", 13);
            hand[4] = new Card("Spades", 12);

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
            int currentBet = AIEvaluate.CurrentBet(actions);
            if (rank == 1)
            {
                if (currentBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "check", 0);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
                }
            }
            else if (rank < 3)
            {
                //if current bet <= 10, call
                //if current bet > 10, fold
                //if no bet, check
                if (currentBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "bet", 10);
                }
                else if (currentBet <= 10)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "call", 0);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
                }
            }
            else if (rank < 5)
            {
                //if current bet < 20, raise to 20
                //if current bet <= 30, call
                //if current bet > 30, fold
                //if no bet, bet 20
                if (currentBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "bet", 20);
                }
                else if (currentBet < 20)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "raise", 20);
                }
                else if (currentBet <= 30)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "call", 0);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
                }
            }
            else if (rank < 6) //stealthy check, to try and draw the opponent in
            {
                //if current bet <= 50, call
                //if current bet > 50, fold
                //if no bet, check
                if (currentBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "check", 0);
                }
                else if (currentBet <= 50)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "call", 0);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
                }
            }
            else //low bet, to try and draw the opponent in
            {
                //if current bet <= 50, raise to 50
                //if current bet > 50, call
                //if no bet, bet 10
                if (currentBet == 0)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "bet", 10);
                }
                else if (currentBet <= 50)
                {
                    pa = new PlayerAction(player.Name, "Bet1", "raise", 50);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet1", "call", 0);
                }
            }

            //pa = new PlayerAction(player.Name, "Bet1", "bet", amount);
            //pa = new PlayerAction(player.Name, "Bet1", "raise", amount);
            //pa = new PlayerAction(player.Name, "Bet1", "check", 0);
            //pa = new PlayerAction(player.Name, "Bet1", "call", 0);
            //pa = new PlayerAction(player.Name, "Bet1", "fold", 0);
            return pa;
        }
    }
}
