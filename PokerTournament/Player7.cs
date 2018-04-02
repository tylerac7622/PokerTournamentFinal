using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{

    //this is the custom player class we have made
    //EVENTUALLY RENAME TO Player7
    class Player7 : Player
    {
        int confidence = 0; //how confident in their hand the player is

        //the constructor of the Player
        public Player7(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
        }
        //the ai handler for the first round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            //hand[0] = new Card("Spades", 10);
            //hand[1] = new Card("Spades", 11);
            //hand[2] = new Card("Diamonds", 12);
            //hand[3] = new Card("Spades", 13);
            //hand[4] = new Card("Spades", 12);

            //list the hand, but only for debugging. EVENTUALLY don't show this
            //AIEvaluate.ListTheHand(hand, player.Name);

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
            int pRank = AIEvaluate.PotentialRank(hand); //can not be less than rank
            //can only be 4 if rank = 2 | can only be 7 if rank = 3 | can only be 8 if rank = 4
            int currentBet = AIEvaluate.CurrentBet(actions);

            int willingBet = 0;
            int willingCheck = 0; //-1 for check on any bet greater than willingBet
            //manipulate the willing values for the ai based on their current cards
            if (rank == 1)
            {
                willingBet = 0;
                willingCheck = 0;
                if (pRank == 2)
                {
                    willingBet += 0;
                    willingCheck += 0;
                }
                else if (pRank == 5)
                {
                    willingBet += 0;
                    willingCheck += 5;
                }
                else if (pRank == 6)
                {
                    willingBet += 5;
                    willingCheck += 10;
                }
                else if (pRank >= 9)
                {
                    willingBet += 10;
                    willingCheck += 10;
                }
            }
            else if (rank == 2)
            {
                willingBet = 5;
                willingCheck = 10;
                if (pRank == 4)
                {
                    willingBet += 0;
                    willingCheck += 5;
                }
                else if (pRank == 5)
                {
                    willingBet += 5;
                    willingCheck += 10;
                }
                else if (pRank == 6)
                {
                    willingBet += 10;
                    willingCheck += 10;
                }
                else if (pRank >= 9)
                {
                    willingBet += 15;
                    willingCheck += 15;
                }
            }
            else if (rank == 3)
            {
                willingBet = 0;
                willingCheck = 30;
                if (pRank == 5)
                {
                    willingBet += 5;
                    willingCheck += 5;
                }
                else if (pRank == 6)
                {
                    willingBet += 5;
                    willingCheck += 10;
                }
                else if (pRank == 7)
                {
                    willingBet += 10;
                    willingCheck += 10;
                }
                else if (pRank >= 9)
                {
                    willingBet += 15;
                    willingCheck += 15;
                }
            }
            else if (rank == 4)
            {
                willingBet = 20;
                willingCheck = 30;
                if (pRank == 5)
                {
                    willingBet += 5;
                    willingCheck += 5;
                }
                else if (pRank == 6)
                {
                    willingBet += 5;
                    willingCheck += 10;
                }
                else if (pRank == 8)
                {
                    willingBet += 10;
                    willingCheck += 10;
                }
                else if (pRank >= 9)
                {
                    willingBet += 15;
                    willingCheck += 15;
                }
            }
            else if (rank == 5)
            {
                willingBet = 20;
                willingCheck = 30;
            }
            else if (rank == 6)
            {
                willingBet = 25;
                willingCheck = 50;
            }
            else if (rank == 7 || rank == 8 || rank == 9 || rank == 10)//low bet, to try and draw the opponent in
            {
                willingBet = 15;
                willingCheck = -1;
                if (currentBet > 10)
                {
                    willingBet = 50;
                }
            }
            //adds a desperation mechanic where the ai will bet more money depending on how little money they have
            float desperation = 1000 / Money;
            desperation = (float)Math.Pow(desperation, .5f);
            willingBet = (int)(willingBet * desperation);
            willingCheck = (int)(willingCheck * desperation);

            //adds a slight randomness to the ai's bet amounts, to make it harder to narrow down what they are actually doing
            Random random = new Random();
            willingBet = (int)(willingBet * ((random.NextDouble() / 5) + .9f)); //multiplied by random double from .9-1.1
            willingCheck = (int)(willingCheck * ((random.NextDouble() / 5) + .9f)); //uses a different random number from the one above
            if (willingBet > willingCheck)
            {
                willingCheck = willingBet;
            }

            //limits the possible bet to the amount of money the ai has (all in)
            if (willingBet > Money)
            {
                willingBet = Money;
                willingCheck = -1;
            }

            //Console.WriteLine("Willing to Bet: " + willingBet);
            //Console.WriteLine("Willing to Check: " + willingCheck);
            //Console.WriteLine();
            //chooses the next action based on what the ai is willing to do
            if (currentBet == 0)
            {
                if (willingBet == 0)
                {
                    pa = new PlayerAction(Name, "Bet1", "check", 0);
                }
                else
                {
                    pa = new PlayerAction(Name, "Bet1", "bet", willingBet);
                }
            }
            else if (currentBet <= willingBet)
            {
                pa = new PlayerAction(Name, "Bet1", "raise", willingBet - currentBet);
            }
            else if (currentBet <= willingCheck || willingCheck == -1)
            {
                pa = new PlayerAction(Name, "Bet1", "call", 0);
            }
            else
            {
                pa = new PlayerAction(Name, "Bet1", "fold", 0);
            }
            return pa;
        }
        //the ai handler for the second round of betting.
        //  actions is all previous actions in the round
        //  hand is the player's current hand
        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            //list the hand, but only for debugging. EVENTUALLY don't show this
            //AIEvaluate.ListTheHand(hand, player.Name);

            PlayerAction pa = null;
            int amount = 10; //the amount to bet or raise by
            int lowConfidence = 20;
            int highConfidence = 60;
            int currentBet = AIEvaluate.CurrentBet(actions);

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

                if (confidence > lowConfidence)
                {
                    amount = confidence;
                    pa = new PlayerAction(Name, "Bet2", "bet", amount);
                }
                else
                {
                    pa = new PlayerAction(Name, "Bet2", "check", 0);
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

                            if (confidence > highConfidence && lastAction.Amount < confidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(Name, "Bet2", "raise", amount);
                            }
                            else if (confidence > lowConfidence || currentBet > 75)
                            {
                                pa = new PlayerAction(Name, "Bet2", "call", 0);
                            }
                            else
                            {
                                pa = new PlayerAction(Name, "Bet2", "fold", 0);
                            }

                        }
                        break;
                    case "check":
                        {
                            //valid options if last action was check for either player are: bet or check

                            if (confidence > lowConfidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(Name, "Bet2", "bet", amount);
                            }
                            else
                            {
                                pa = new PlayerAction(Name, "Bet2", "check", 0);
                            }
                        }
                        break;
                    case "raise":
                        {
                            //valid options if last action was raise for either player are: raise, call, or fold

                            if (confidence > highConfidence && lastAction.Amount < confidence)
                            {
                                amount = confidence;
                                pa = new PlayerAction(Name, "Bet2", "raise", amount);
                            }
                            else if (confidence > lowConfidence || currentBet > 75)
                            {
                                pa = new PlayerAction(Name, "Bet2", "call", 0);
                            }
                            else
                            {
                                pa = new PlayerAction(Name, "Bet2", "fold", 0);
                            }
                        }
                        break;
                }
            }

            return pa;
        }

        /// <summary>
        /// Checks how confident the AI should be based upon their hand
        /// </summary>
        /// <param name="hand">The five cards the player is currently holding</param>
        private void CheckConfidence(Card[] hand)
        {
            Card highCard;
            confidence = 10 * (Evaluate.RateAHand(hand, out highCard) - 1);

            //Hand can be rated 1- 10 (inclusive)
            //therefore confidence can be 0 - 90 in 10 piece increments
            //Add the value of the high card to that score

            //need to adjust confidence by the value of the cards if of the following type: highcard, 1-4 pairs, straight, flush

            //first if it is a flush, straight, or straight flush take the highest card as its value and add it to the confidence
            if (AIEvaluate.AllSameSuit(hand, highCard.Suit) == 5 || confidence == 40 || confidence == 80)
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
        //the ai handler for the discard/draw phase between the betting rounds.
        //  hand is the player's current hand
        public override PlayerAction Draw(Card[] hand)
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
            //AIEvaluate.ListTheHand(hand, player.Name);

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
                        //Console.WriteLine(hand[i].Suit);
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

            pa = new PlayerAction(Name, "Draw", action, numCardsToDiscard);
            return pa;
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
