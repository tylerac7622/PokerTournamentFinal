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
        // the ai handler for the draw.
        // hand is the player's current hand
        public PlayerAction Draw(Card[] hand, PlayerN player)
        {
            //list the hand, but only for debugging. EVENTUALLY don't show this
            AIEvaluate.ListTheHand(hand, player.Name);

            string action = "draw";
            PlayerAction pa = null;
            int cardsToDiscard = 3; //max 5

            ///Possible actions
            ///  stand pat
            ///  draw - requires number of cards to draw
            ///      BEFORE DRAWING - choose which cards to discard
            
            // get rank of hand (this will sort the hand in the process)
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // Random object to reuse based on current rank
            Random random = new Random();
            float percent = 0.0f;

            // hands this good should be kept
            if (rank >= 5)
                cardsToDiscard = 0;
            else if (rank == 4)
            {
                // three of a kind - never discard more than 2 cards
                percent = random.Next(1);
                float percentForTwo = 0.5f;

                // discard 1
                cardsToDiscard = 1;

                // discard 2
                if (percent < percentForTwo)
                    cardsToDiscard = 2;
            }
            else if (rank == 3)
                // two pairs - just get rid of the last card
                cardsToDiscard = 1;
            else if (rank == 2)
                // one pair - discard three
                cardsToDiscard = 3;
            else
            {
                // high card - discard at least three
                cardsToDiscard = 3;
                percent = random.Next(1);

                if (percent < 0.33f)
                {
                    cardsToDiscard = 4;
                }
                else if (percent < 0.66f)
                {
                    cardsToDiscard = 5;
                }
            }

            // if no cards were chosen to discard
            // this means the player has decided to stand pat
            if (cardsToDiscard == 0)
                action = "stand pat";

            for (int i = 0; i < cardsToDiscard; i++)
            {
                hand[i] = null;
            }
            pa = new PlayerAction(player.Name, "Draw", action, cardsToDiscard);
            return pa;
        }
    }
}
