using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    //TEMP FILE SO WE CAN WORK ON THINGS AT THE SAME TIME
    class TEMPBettingRound2
    {
        int confidence = 0; //how confident in their hand the player is

        //the ai handler for the second round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand, PlayerN player)
        {
            //list the hand, but only for debugging. EVENTUALLY don't show this
            AIEvaluate.ListTheHand(hand, player.Name);

            PlayerAction pa = null;
            int amount = 10; //the amount to bet or raise by
            int lowConfidence = 20;
            int highConfidence = 60;

            ///Possible actions
            ///  bet - requires amount - cannot bet unless no previous bets in round (going first or only checks before)
            ///  raise - requires amount - cannot raise unless previous action was either bet or check (or fold for >2 players)
            ///  check - cannot check unless betting first or unless previous actions were checks
            ///  call - cannot call unless betting second and bet or raise was done previously
            ///  fold
            ///      Doing any action at the wrong time defaults to fold (player sacrifices their hand)
            ///      

            //get the last action
            PlayerAction lastAction = actions[actions.Count - 1];

            //determine how confident the player should be in their hand
            CheckConfidence(hand);

            //check what round the previous action was done during
            if (lastAction.ActionPhase == "Draw")
            {
                //valid options if last action was draw for either player are: bet or check

                if(confidence > lowConfidence)
                {
                    amount = confidence;
                    pa = new PlayerAction(player.Name, "Bet2", "bet", amount);
                }
                else
                {
                    pa = new PlayerAction(player.Name, "Bet2", "check", 0);
                }
                
            }
            else
            {
                //valid options if the last action was part of betting round 2: all

                //if the other player called or folded the program goes to showdown or ends
                switch (lastAction.ActionName)
                {
                    case "bet":
                        {
                            //valid options if last action was bet for either player are: raise, call, or fold

                            if (confidence > highConfidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(player.Name, "Bet2", "raise", amount);
                            }
                            else if(confidence > lowConfidence)
                            {
                                pa = new PlayerAction(player.Name, "Bet2", "call", 0);
                            }
                            else
                            {
                                pa = new PlayerAction(player.Name, "Bet2", "fold", 0);
                            }
                            
                        }
                        break;
                    case "check":
                        {
                            //valid options if last action was check for either player are: bet or check

                            if (confidence > lowConfidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(player.Name, "Bet2", "bet", amount);
                            }
                            else
                            {
                                pa = new PlayerAction(player.Name, "Bet2", "check", 0);
                            }
                        }
                        break;
                    case "raise":
                        {
                            //valid options if last action was raise for either player are: raise, call, or fold

                            if (confidence > highConfidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(player.Name, "Bet2", "raise", amount);
                            }
                            else if (confidence > lowConfidence)
                            {
                                pa = new PlayerAction(player.Name, "Bet2", "call", 0);
                            }
                            else
                            {
                                pa = new PlayerAction(player.Name, "Bet2", "fold", 0);
                            }
                        }
                        break;
                }
            }

            return pa;
        }


        private void CheckConfidence(Card[] hand)
        {
            Card highCard;
            confidence = 10*(Evaluate.RateAHand(hand, out highCard)-1);

            //Hand can be rated 1- 10 (inclusive)
            //therefore confidence can be 0 - 90 in 10 piece increments
            //Add the value of the high card to that score

            //need to adjust confidence by the value of the cards if of the following type: highcard, 1-4 pairs, straight, flush

            //first if it is a flush, straight, or straight flush take the highest card as its value and add it to the confidence
            if(AIEvaluate.AllSameSuit(hand, highCard.Suit) == 5 || confidence == 40 || confidence == 80)
            {
                confidence += highCard.Value;
            }
            else //otherwise make sure to get the biggest grouping of like values and then add the value of one of those to the confidence
            {
                List<List<Card>> temp = AIEvaluate.MostOfSameValueCards(hand);
                List<Card> temp1 = temp[temp.Count - 1];
                confidence += temp1[temp1.Count - 1].Value;
            }
            
        }
    }
}
