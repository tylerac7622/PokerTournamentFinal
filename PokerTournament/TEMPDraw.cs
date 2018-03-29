using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    //TEMP FILE SO WE CAN WORK ON THINGS AT THE SAME TIME
    class TEMPDraw
    {
        //the ai handler for the second round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public PlayerAction Draw(Card[] hand, PlayerN player)
        {
            //list the hand, but only for debugging. EVENTUALLY don't show this
            //AIEvaluate.ListTheHand(hand, player.Name);

            PlayerAction pa = null;
            int cardsToDiscard = 3; //max 5
            ///Possible actions
            ///  stand pat
            ///  draw - requires number of cards to draw
            ///      BEFORE DRAWING - choose which cards to discard
            ///      
            for (int i = 0; i < cardsToDiscard; i++)
            {
                hand[i] = null;
            }
            pa = new PlayerAction(player.Name, "Draw", "draw", cardsToDiscard);
            //pa = new PlayerAction(player.Name, "Draw", "stand pat", 0);
            return pa;
        }
    }
}
