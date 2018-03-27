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
        //the ai handler for the second round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand, PlayerN player)
        {
            //list the hand, but only for debugging. EVENTUALLY don't show this
            ListTheHand(hand, player);
            PlayerAction pa = null;
            int amount = 10;
            ///Possible actions
            ///  bet - requires amount - cannot bet unless no previous bets in round (going first or only checks before)
            ///  raise - requires amount - cannot raise unless previous action was either bet or check (or fold for >2 players)
            ///  check - cannot check unless betting first or unless previous actions were checks
            ///  call - cannot call unless betting second and bet or raise was done previously
            ///  fold
            ///      Doing any action at the wrong time defaults to fold (player sacrifices their hand)
            ///      
            //pa = new PlayerAction(Name, "Bet2", "bet", amount);
            //pa = new PlayerAction(Name, "Bet2", "raise", amount);
            pa = new PlayerAction(player.Name, "Bet2", "check", 0);
            //pa = new PlayerAction(Name, "Bet2", "call", 0);
            //pa = new PlayerAction(Name, "Bet2", "fold", 0);
            return pa;
        }

        private void ListTheHand(Card[] hand, PlayerN player)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.Write("\nName: " + player.Name + "\n\tRank: " + rank + "\n\tTheir hand:");
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write("\n\t " + hand[i].ToString() + " ");
            }
            Console.WriteLine();
        }
    }
}
