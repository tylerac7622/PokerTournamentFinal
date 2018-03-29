using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{

    //this is the custom player class we have made
    //EVENTUALLY RENAME TO Player7
    class PlayerN : Player
    {
        TEMPBettingRound1 temp1 = new TEMPBettingRound1();
        TEMPBettingRound2 temp2 = new TEMPBettingRound2();
        TEMPDraw tempDraw = new TEMPDraw();
        //the constructor of the Player
        public PlayerN(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
        }
        //the ai handler for the first round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            return temp1.BettingRound1(actions, hand, this);
        }
        //the ai handler for the second round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            return temp2.BettingRound2(actions, hand, this);
        }
        //the ai handler for the discard/draw phase between the betting rounds.
        //  hand is the player's current hand
        public override PlayerAction Draw(Card[] hand)
        {
            return tempDraw.Draw(hand, this);
        }
        
        private void ListTheHand(Card[] hand)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.Write("\nName: " + Name + "\n\tRank: " + AIEvaluate.PrintRank(rank) + "\n\tTheir hand:");
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write("\n\t " + hand[i].ToString() + " ");
            }
            Console.WriteLine();
        }
    }
}
