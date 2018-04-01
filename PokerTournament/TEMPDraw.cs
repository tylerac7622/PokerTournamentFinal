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
            // DEBUG HAND
            // test potential flush from one pair
            //hand[0] = new Card("Hearts", 14);
            //hand[1] = new Card("Spades", 14);
            //hand[2] = new Card("Hearts", 2);
            //hand[3] = new Card("Hearts", 4);
            //hand[4] = new Card("Hearts", 9);

            // test potential straight from one pair
            //hand[0] = new Card("Hearts", 5);
            //hand[1] = new Card("Spades", 4);
            //hand[2] = new Card("Clubs", 3);
            //hand[3] = new Card("Diamonds", 2);
            //hand[4] = new Card("Hearts", 2);

            //list the hand, but only for debugging. EVENTUALLY don't show this
            AIEvaluate.ListTheHand(hand, player.Name);

            string action = "draw";
            PlayerAction pa = null;
            int numCardsToDiscard = 3; //max 5

            ///Possible actions
            ///  stand pat
            ///  draw - requires number of cards to draw
            ///      BEFORE DRAWING - choose which cards to discard

            // get rank of hand (this will sort the hand in the process)
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);
            List<Card> cardsToDiscard = new List<Card>();
            int pRank = AIEvaluate.PotentialRank(hand);

            // PROBABILITIES (OUT OF 1) OF GETTING A GIVEN HAND
            // source: https://math.hawaii.edu/~ramsey/Probability/PokerHands.html
            // rank 01, high card:       0.501177
            // rank 02, one pair:        0.422569
            // rank 03, two pairs:       0.047539
            // rank 04, 3 of a kind:     0.021128
            // rank 05, straight:        0.00392465
            // rank 06, flush:           0.0019654
            // rank 07, full house:      0.001441
            // rank 08, 4 of a kind:     0.000240
            // rank 09, straight flush:  0.0000138517
            // rank 10, royal flush:     0.00000153908

            // Random object to reuse based on current rank
            Random random = new Random();
            double percent = 0.0f;
            //percent = random.NextDouble();
            
            // hands this good should be kept (they each have a <1% chance of occuring!)
            if (rank >= 5)
            {
                numCardsToDiscard = 0;
            }
            else if (rank == 4)
            {
                // three of a kind - never discard more than 2 cards
                percent = random.NextDouble();
                float percentFor2 = 0.95f;

                // discard 1
                numCardsToDiscard = 1;

                // discard 2
                if (percent < percentFor2)
                {
                    numCardsToDiscard = 2;
                }

                // at this point, the hand is not a full house,
                // so the two cards that are not one of the three
                // are unique from each other.
                for (int i = 0; i < hand.Length; i++)
                {
                    // get # of that value in hand
                    if (Evaluate.ValueCount(hand[i].Value, hand) == 1 && cardsToDiscard.Count < numCardsToDiscard)
                    {
                        cardsToDiscard.Add(hand[i]);
                    }
                }
            }
            else if (rank == 3)
            {
                // two pairs - just get rid of the last card
                numCardsToDiscard = 1;

                // unlikely, but get rid of 3 if feeling lucky
                percent = random.NextDouble();
                float percentFor3 = 0.01f;
                if (percent < percentFor3)
                {
                    numCardsToDiscard = 3;
                }

                // only discard the card that isn't in either pair
                for (int i = 0; i < hand.Length; i++)
                {
                    if (Evaluate.ValueCount(hand[i].Value, hand) == 1)
                    {
                        cardsToDiscard.Add(hand[i]);
                    }
                }

                // discard the lower of the two pairs if discarding 3
                if (numCardsToDiscard == 3)
                {
                    // get the values of each pair
                    List<int> pairValues = new List<int>();
                    for (int i = 0; i < hand.Length; i++)
                    {
                        // DEBUG
                        //Console.WriteLine(hand[i].Value);

                        if (Evaluate.ValueCount(hand[i].Value, hand) == 2 && !pairValues.Contains(hand[i].Value))
                        {
                            pairValues.Add(hand[i].Value);
                        }
                    }

                    // DEBUG
                    //Console.WriteLine("num vals of pairs = " + pairValues.Count);
                    //for (int i = 0; i < pairValues.Count; i++)
                    //{
                    //    Console.WriteLine("value at " + i + " = " + pairValues[i]);
                    //}

                    // get the value of the lower of the two pairs
                    int lower = pairValues[0];
                    if (pairValues[1] < lower)
                    {
                        lower = pairValues[1];
                    }

                    // DEBUG
                    //Console.WriteLine("lower val = " + lower);

                    // discard cards in lower pair
                    for (int i = 0; i < hand.Length; i++)
                    {
                        if (Evaluate.ValueCount(hand[i].Value, hand) == 2 && hand[i].Value == lower)
                        {
                            cardsToDiscard.Add(hand[i]);
                        }
                    }
                } 
            }
            else if (rank == 2)
            {
                // one pair - discard 3
                numCardsToDiscard = 3;

                // if close to straight, get rid of one element of pair
                if (pRank == 5)
                {
                    numCardsToDiscard = 1;
                    bool hasPair = false;
                    for (int i = 0; i < hand.Length; i++)
                    {
                        if (Evaluate.ValueCount(hand[i].Value, hand) == 2 && !hasPair)
                        {
                            cardsToDiscard.Add(hand[i]);
                            hasPair = true;
                        }
                    }
                }
                // if close to flush, delete card(s) with problem suit
                else if (pRank == 6)
                {
                    Dictionary<string, int> suitCount = new Dictionary<string, int>();
                    suitCount.Add("Spades", 0);
                    suitCount.Add("Clubs", 0);
                    suitCount.Add("Hearts", 0);
                    suitCount.Add("Diamonds", 0);

                    // get number of cards in each suit
                    for (int i = 0; i < hand.Length; i++)
                    {
                        Console.WriteLine(hand[i].Suit);
                        suitCount[hand[i].Suit]++;
                    }

                    // get the suit that accounts for the flush
                    string majoritySuit = "Spades";
                    foreach (string suit in suitCount.Keys)
                    {
                        if (suitCount[suit] > suitCount[majoritySuit])
                        {
                            majoritySuit = suit;
                        }
                    }

                    for (int i = 0; i < hand.Length; i++)
                    {
                        // get rid of card if not in correct suit
                        if (hand[i].Suit != majoritySuit)
                        {
                            cardsToDiscard.Add(hand[i]);
                        }
                    }
                    numCardsToDiscard = cardsToDiscard.Count;
                }
                // normal case
                else
                {
                    // discard the 3 cards not in the pair
                    for (int i = 0; i < hand.Length; i++)
                    {
                        int count = Evaluate.ValueCount(hand[i].Value, hand);

                        // DEBUG
                        //Console.WriteLine(count);
                        
                        if (Evaluate.ValueCount(hand[i].Value, hand) == 1)
                        {
                            cardsToDiscard.Add(hand[i]);
                        }
                    }
                }
            }
            else
            {
                // high card

                // mainly based on potential rank
                // if close to something good and rare, just get rid of problem card(s)
                if (pRank >= 5)
                {
                    List<List<Card>> goodCards = new List<List<Card>>();
                    switch (pRank)
                    {
                        case 10:
                            goodCards = AIEvaluate.CloseToRoyalFlushCards(hand);
                            break;
                        case 9:
                            goodCards = AIEvaluate.CloseToStraightFlushCards(hand);
                            break;
                        case 8:
                            // if close to 4 of a kind, it's 3 of a kind
                            break;
                        case 7:
                            // if close to full house, it's 3 of a kind or 2 pairs
                            break;
                        case 6:
                            goodCards = AIEvaluate.CloseToFlushCards(hand);
                            break;
                        case 5:
                            goodCards = AIEvaluate.CloseToStraightCards(hand);
                            break;
                        default: break;
                    }

                    // only get rid of problem cards for potential cases that can result from a high card hand
                    if (pRank < 7 || pRank > 8)
                    {

                        for (int i = 0; i < hand.Length; i++)
                        {
                            for (int j = 0; j < goodCards.Count; j++)
                            {
                                if (!goodCards[j].Contains(hand[i]))
                                {
                                    cardsToDiscard.Add(hand[i]);
                                }
                            }
                        }
                    }

                    // properly set number of discarding cards
                    numCardsToDiscard = cardsToDiscard.Count;
                }
                else
                {
                    // otherwise, just get rid of at least 3 cards randomly

                    numCardsToDiscard = 3;
                    percent = random.NextDouble();

                    float percentFor4 = 0.33f;
                    float percentFor5 = 0.33f;
                    percentFor5 += percentFor4;

                    // discard 4
                    if (percent < percentFor4)
                    {
                        numCardsToDiscard = 4;
                    }
                    // discard all 5
                    else if (percent < percentFor5)
                    {
                        numCardsToDiscard = 5;
                    }

                    // since the hand is sorted
                    // just discard the lowest cards in the hand
                    for (int i = 0; i < numCardsToDiscard; i++)
                    {
                        cardsToDiscard.Add(hand[i]);
                    }
                }
            }

            // if no cards were chosen to discard
            // this means the player has decided to stand pat
            if (numCardsToDiscard == 0)
            {
                action = "stand pat";
            }
            else
            {
                // new
                // go through cardsToDiscard and remove its cards from hand
                for (int i = 0; i < hand.Length; i++)
                {
                    if (cardsToDiscard.Contains(hand[i]))
                    {
                        hand[i] = null;
                    }
                }
            }

            pa = new PlayerAction(player.Name, "Draw", action, numCardsToDiscard);
            return pa;
        }
    }
}
