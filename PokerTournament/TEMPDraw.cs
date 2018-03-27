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
            PlayerAction pa = null;
            int cardsToDiscard = 0; //max 5
            ///Possible actions
            ///  stand pat
            ///  draw - requires number of cards to draw
            ///      BEFORE DRAWING - choose which cards to discard
            ///      
            /*for (int i = 0; i < cardsToDiscard; i++)
            {
                hand[i] = null;
            }
            pa = new PlayerAction(Name, "Draw", "draw", cardsToDiscard);*/
            pa = new PlayerAction(player.Name, "Draw", "stand pat", 0);
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
