using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnalyzer
{
    //The hand ignores the last twelve bits of the ulong,
    private const int HAND_OFFSET = 12;
    private const ulong FOUR_OF_A_KIND_MASK = 0b1000000000000100000000000010000000000001000000000000;
    private const ulong FLUSH_MASK = 0b1111111111111000000000000000000000000000000000000000;
    private const ulong CARD_MASK = 0b1000000000000000000000000000000000000000000000000000;
    private const ulong A2345_STRAIGHT_FLUSH_MASK = 0b100000000111100000000000000000000000000000000000000;
    private const ulong STRAIGHT_FLUSH_MASK = 0b111110000000000000000000000000000000000000000000000;
    private ulong _binaryHand;

    public HandAnalyzer()
    {
        //Do nothing, as there's nothing to initialize
    }

    ///<summary>
    ///Card suits are grouped into 13 bit segments, going S -> H -> D -> C.
    ///The first rank in each segment is the Ace, and the last rank is 2.
    ///</summary>
    ///<returns>ulong with a single bit set, representing the given card.</returns>
    ///<param name="card">The card that will be converted into a bit.</param>
    private ulong ConvertCardToBinary(Card card)
    {
        int rank = (int)card.face;
        int suit = (int)card.suit;

        ulong binaryCard = 1;
        //Automatically bitshift left by 12 position
        int shift = HAND_OFFSET + (13 * suit) + rank;
        binaryCard = binaryCard << shift;

        return binaryCard;
    }

    ///<summary>
    ///Convert the given hand to a 52-bit number, each bit representing a card.
    ///</summary>
    ///<param name="hand">The hand that will be converted into a 52-bit number.</param>
    private void CreateBinaryHand(Hand hand)
    {
        _binaryHand = 0;
        foreach (Card card in hand.cards)
        {
            _binaryHand |= ConvertCardToBinary(card);
        }
    }


    ///<summary>
    ///Determine the best hand that can be derived from a given hand and community pool.
    ///</summary>
    ///<returns>An array of integers representing card indexes and a name for the hand.</returns>
    ///<param name="hand">The hand that will be evaluated.</param>
    ///<param name="pool">The pool of community cards that will be considered when determining a hand.</param>
    public (int[], HandName) DetermineHand(Hand hand, CommunityPool pool = null)
    {
        if (pool == null)
            CreateBinaryHand(hand);

        //Check for hands in descending priority
        //Royal Flush -> Straight Flush -> Four of a Kind -> Full House -> Flush -> Straight -> Three of a Kind -> Two Pair -> Pair -> High Card
        int[] bestPossibleHand;

        if ((bestPossibleHand = HasRoyalFlush()) != null)
        {
            return (bestPossibleHand, HandName.RoyalFlush);
        }

        else if ((bestPossibleHand = HasStraightFlush()) != null)
        {
            return (bestPossibleHand, HandName.StraightFlush);
        }

        else if ((bestPossibleHand = HasFourOfAKind()) != null)
        {
            //Determine the rank of the hand 
            int rank = bestPossibleHand[0] % 13;
            int[] completeHand = CompleteHand(1, rank);
            return (new int[] { bestPossibleHand[0], bestPossibleHand[1], bestPossibleHand[2], bestPossibleHand[3], completeHand[0] },
                HandName.FourOfAKind);
        }

        else if ((bestPossibleHand = HasFullHouse()) != null)
        {
            return (bestPossibleHand, HandName.FullHouse);
        }

        else if ((bestPossibleHand = HasFlush()) != null)
        {
            return (bestPossibleHand, HandName.Flush);
        }

        else if ((bestPossibleHand = HasStraight()) != null)
        {
            return (bestPossibleHand, HandName.Straight);
        }

        else if ((bestPossibleHand = HasThreeOfAKind()) != null)
        {
            int rank = bestPossibleHand[0] % 13;
            int[] completeHand = CompleteHand(2, rank);
            return (new int[] { bestPossibleHand[0], bestPossibleHand[1], bestPossibleHand[2], completeHand[0], completeHand[1] },
                HandName.ThreeOfAKind);
        }

        else if ((bestPossibleHand = HasTwoPair()) != null)
        {
            int rank1 = bestPossibleHand[0] % 13;
            int rank2 = bestPossibleHand[2] % 13;
            int[] completeHand = CompleteHand(1, rank1, rank2);
            return (new int[] { bestPossibleHand[0], bestPossibleHand[1], bestPossibleHand[2], bestPossibleHand[3], completeHand[0] },
                HandName.TwoPair);
        }

        else if ((bestPossibleHand = HasPair()) != null)
        {
            int rank = bestPossibleHand[0] % 13;
            int[] completeHand = CompleteHand(3, rank);
            return (new int[] { bestPossibleHand[0], bestPossibleHand[1], completeHand[0], completeHand[1], completeHand[2] },
                HandName.Pair);
        }

        //If the contestant's hand did not match any pattern associated with any of the groups above, then build their best hand from the 5 highest ranking cards they have
        else
        {
            int[] completeHand = CompleteHand(5);
            return (completeHand, HandName.High);
        }
    }

    ///<summary>
    ///Check for a Royal Flush.
    ///</summary>
    ///<returns>An array of the indeces representing cards.</returns>
    private int[] HasRoyalFlush()
    {
        ulong royalFlush = 31; //Represented as 000...11111

        //Clubs
        royalFlush <<= 20;
        if ((_binaryHand & royalFlush) == royalFlush)
            return new int[] { 8, 9, 10, 11, 12 };

        //Diamonds
        royalFlush <<= 13;
        if ((_binaryHand & royalFlush) == royalFlush)
            return new int[] { 21, 22, 23, 24, 25 };

        //Hearts
        royalFlush <<= 13;
        if ((_binaryHand & royalFlush) == royalFlush)
            return new int[] { 34, 35, 36, 37, 38 };

        //Spades
        royalFlush <<= 13;
        if ((_binaryHand & royalFlush) == royalFlush)
            return new int[] { 47, 48, 49, 50, 51 };

        return null;
    }

    ///<summary>
    ///Check for a straight flush.
    ///</summary>
    ///<returns>An array of the indeces representing cards.</returns>
    private int[] HasStraightFlush()
    {
        //ulong straightFlush = 31 << 58; //Represented as 000...11111
        ulong straightFlush = STRAIGHT_FLUSH_MASK;
        //straightFlush <<= 58; //Shift to the 5 highest order bits
        //ulong A2345 = 4111;
        //A2345 <<= 51; //Represented by pattern 1000000001111
        ulong A2345 = A2345_STRAIGHT_FLUSH_MASK;


        for (int suit = 3; suit >= 0; --suit)
        {
            //Begin at the highest rank, Ace, and work down to the lowest rank where there can be a contiguous straight, 6
            for (int rank = 12; rank >= 4; --rank)
            {
                //If a straight flush is found, then return the highest rank in the hand
                if ((_binaryHand & straightFlush) == straightFlush)
                {
                    int highCard = 13 * suit + rank;
                    return new int[] { highCard, highCard - 1, highCard - 2, highCard - 3, highCard - 4 };
                }
                else
                    straightFlush >>= 1;
            }

            if ((_binaryHand & A2345) == A2345)
            {
                int fiveCard = 13 * suit + 3;
                int aceCard = 13 * suit + 12;
                return new int[] { aceCard, fiveCard - 3, fiveCard - 2, fiveCard - 1, fiveCard };
            }
            else
            {
                //Shift right enough to enter the next suit
                straightFlush >>= 4;
                A2345 >>= 13;
            }
        }

        return null;
    }

    ///<summary>
    ///Check for a Four of a Kind.
    ///</summary>
    ///<returns>An array of the indeces representing cards.</returns>
    private int[] HasFourOfAKind()
    {
        /*
        //Shift a total of 63 bits
        ulong fourOfAKind = 1;
        fourOfAKind <<= 13;
        fourOfAKind |= 1;
        fourOfAKind <<= 13;
        fourOfAKind |= 1;
        fourOfAKind <<= 13;
        fourOfAKind |= 1;
        fourOfAKind <<= 13;
        //Shift left an additional 11 bits to account for the length of the ulong
        fourOfAKind <<= 11;
        */
        ulong fourOfAKind = FOUR_OF_A_KIND_MASK;

        //Begin looking for four Aces, proceeding to four Twos
        for (int rank = 12; rank >= 0; --rank)
        {
            if ((_binaryHand & fourOfAKind) == fourOfAKind)
                return new int[] { rank, rank + 13, rank + 26, rank + 39 };
            else
                fourOfAKind >>= 1;
        }

        return null;
    }

    ///<summary>
    ///Check for a Three of a Kind.
    ///</summary>
    ///<returns>An array of the indeces representing cards.</returns>
    private int[] HasThreeOfAKind()
    {
        ulong threeOfAKindBase = 1;
        threeOfAKindBase <<= 63;
        ulong cardMask;
        int first = -1, second = -1, third = -1;

        for (int rank = 12; rank >= 0; --rank)
        {

            cardMask = threeOfAKindBase;
            for (int suit = 3; suit >= 0; --suit)
            {
                //If the rank occurs within a suit, then increment the counter
                if ((_binaryHand & cardMask) == cardMask)
                {
                    if (first != -1)
                    {
                        if (second != -1)
                        {
                            third = suit * 13 + rank;
                            return new int[] { first, second, third };
                        }
                        else
                        {
                            second = suit * 13 + rank;
                        }
                    }
                    else
                    {
                        first = suit * 13 + rank;
                    }
                }

                //Move to the next suit
                cardMask >>= 13;
            }

            //Reset the counter for the next rank
            first = -1;
            second = -1;

            threeOfAKindBase >>= 1;
        }

        return null;
    }

    ///<summary>
    ///Check for a Pair.
    ///</summary>
    ///<returns>An array of the indeces representing cards.</returns>
    private int[] HasPair()
    {
        ulong pairBase = 1;
        pairBase <<= 63;
        ulong cardMask;
        int first = -1, second = -1;

        for (int rank = 12; rank >= 0; --rank)
        {
            cardMask = pairBase;
            for (int suit = 3; suit >= 0; --suit)
            {
                //If the rank occurs within a suit, then increment the counter
                if ((_binaryHand & cardMask) == cardMask)
                {
                    if (first != -1)
                    {
                        second = suit * 13 + rank;
                        return new int[] { first, second };
                    }
                    else
                    {
                        first = suit * 13 + rank;
                    }
                }

                //Move to the next suit
                cardMask >>= 13;
            }
            //Reset the counter for the next rank
            first = -1;

            pairBase >>= 1;
        }

        return null;
    }


    ///<summary>
    ///Check for a Two Pair.
    ///</summary>
    ///<returns>An array of the indeces representing cards in the Two Pair.</returns>
    private int[] HasTwoPair()
    {
        ulong pairMask = 1;
        pairMask <<= 63;
        ulong cardMask;
        (int, int) firstPair = (-1, -1);
        (int, int) secondPair = (-1, -1);
        int rankBreak = -1;

        for (int rank = 12; rank >= 0; --rank)
        {
            cardMask = pairMask;
            for (int suit = 3; suit >= 0; --suit)
            {
                //If the rank occurs within a suit, then increment the counter
                if ((_binaryHand & cardMask) == cardMask)
                {
                    if (firstPair.Item1 != -1)
                    {
                        firstPair.Item2 = suit * 13 + rank;
                        break;
                    }
                    else
                    {
                        firstPair.Item1 = suit * 13 + rank;
                    }
                }
                //Move to the next suit
                cardMask >>= 13;
            }

            //Reset the counter for the next rank
            pairMask >>= 1;
            if (firstPair.Item2 != -1)
            {
                //rankBreak is the rank of the first pair. Searching for the second pair will begin at the next rank
                rankBreak = rank;
                break;
            }
            else
            {
                //If no pair was discovered for the given rank, then reset item1 to -1
                firstPair.Item1 = -1;
            }
        }

        //rankBreak will be -1 if no pair was found
        if (rankBreak == -1)
            return null;

        //This loop is identical to the one above. It resumes where the other broke off and looks to the second pair.
        for (int rank = rankBreak - 1; rank >= 0; --rank)
        {
            cardMask = pairMask;
            for (int suit = 3; suit >= 0; --suit)
            {
                if ((_binaryHand & cardMask) == cardMask)
                {
                    if (secondPair.Item1 != -1)
                    {
                        secondPair.Item2 = suit * 13 + rank;
                        return new int[] { firstPair.Item1, firstPair.Item2, secondPair.Item1, secondPair.Item2 };
                    }
                    else
                    {
                        secondPair.Item1 = suit * 13 + rank;
                    }
                }
                cardMask >>= 13;
            }
            pairMask >>= 1;
            //If the rank changes, then no pair was foumd, so automatically reset item1 to 0
            secondPair.Item1 = -1;
        }

        return null;
    }

    ///<summary>
    ///Check for a Full House.
    ///</summary>
    ///<returns><returns>An array of the indeces representing cards in the Full House.</returns>
    private int[] HasFullHouse()
    {
        ulong fullHouseBase = 1;
        fullHouseBase <<= 63;
        ulong cardMask;
        int first = -1, second = -1, third = -1;
        int[] pair = null;
        int[] trio = null;

        for (int rank = 12; rank >= 0; --rank)
        {
            cardMask = fullHouseBase;
            for (int suit = 3; suit >= 0; --suit)
            {
                //If the rank occurs within a suit, then increment the counter
                if ((_binaryHand & cardMask) == cardMask)
                {
                    if (first != -1)
                    {
                        if (second != -1)
                        {
                            third = suit * 13 + rank;
                        }
                        else
                        {
                            second = suit * 13 + rank;
                        }
                    }
                    else
                    {
                        first = suit * 13 + rank;
                    }
                }

                cardMask >>= 13;
            }

            //If a second match was found, but not a third, then there is a pair
            if (second != -1 && third == -1)
            {
                pair = new int[] { first, second };
            }
            //If a third match was found, then there is a three of a kind
            else if (third != -1)
            {
                trio = new int[] { first, second, third };
            }

            if (pair != null && trio != null)
            {
                return new int[] { pair[0], pair[1], trio[0], trio[1], trio[2] };
            }

            fullHouseBase >>= 1;

            //Reset the variables when moving to the next rank
            first = -1; second = -1; third = -1;
        }

        return null;
    }

    ///<summary>
    ///Check for a Flush.
    ///</summary>
    ///<returns>An array of the indeces representing cards in the Flush.</returns>
    private int[] HasFlush()
    {
        //Maks all 13 bits belonging to a suit
        //ulong flushMask = 8191; //Represented as 1111111111111 (13 bits)
        ulong flushMask = FLUSH_MASK;
        ulong rankMask = 1;
        int counter = 0;
        //flushMask <<= 51;
        rankMask <<= 63;
        int[] values = new int[5] { -1, -1, -1, -1, -1 };


        for (int suit = 3; suit >= 0; --suit)
        {
            ulong result = _binaryHand & flushMask;
            for (int rank = 12; rank >= 0; --rank)
            {
                if ((result & rankMask) == rankMask)
                {
                    //Add the rank to the array
                    values[counter++] = suit * 13 + rank;
                }

                rankMask >>= 1;
            }

            //If the array is full, then return the contents because there is a flush
            if (values[4] != -1)
                return values;
            //Else, there is not a flush, so reset the array
            else
                values[0] = values[1] = values[2] = values[3] = -1;


            counter = 0;
            //Shift the mask over to cover the next suit
            flushMask >>= 13;
        }

        return null;
    }

    ///<summary>
    ///Check for a Straight.
    ///</summary>
    ///<returns>An array of the indeces representing cards in the Straight.</returns>
    private int[] HasStraight()
    {
        //rankMask sets all the bits of the rank being investigated
        ulong cardBase = 1;
        cardBase <<= 63;
        ulong cardMask;

        int first = 0, second = 0, third = 0, fourth = 0, fifth = 0;
        int streak = 0;
        int acePresent = 0;

        for (int rank = 12; rank >= 0; --rank)
        {
            cardMask = cardBase;
            for (int suit = 3; suit >= 0; --suit)
            {
                //For each card found to be in a streak, save the card as an integer
                if ((_binaryHand & cardMask) == cardMask)
                {
                    //If an Ace is detected, save its index. It may be used to find a A2345 straight.
                    if (rank == 12)
                        acePresent = 13 * suit + rank;

                    ++streak;
                    if (streak == 1)
                        first = 13 * suit + rank;
                    else if (streak == 2)
                        second = 13 * suit + rank;
                    else if (streak == 3)
                        third = 13 * suit + rank;
                    else if (streak == 4)
                        fourth = 13 * suit + rank;
                    else if (streak == 5)
                    {
                        fifth = 13 * suit + rank;
                        return new int[] { first, second, third, fourth, fifth };
                    }
                    break;
                }
                //If the streak is broken, then return the counter to 0, and start counting with the next rank
                else if (suit == 0)
                {
                    streak = 0;
                }

                //Check the next suit
                cardMask >>= 13;
            }

            //Move to the next rank to check
            cardBase >>= 1;

        }

        //If the loop ended, and there is a streak of 4, that means the ranks are 5 4 3 2. Check if an Ace was detected earlier to complete the straight.
        if (streak == 4 && acePresent > 0)
        {
            fifth = acePresent;
            return new int[] { first, second, third, fourth, fifth };
        }

        return null;
    }

    ///<summary>
    ///Check for cards in order of declining value, beginning with Ace and proceeding to Two.
    ///A specific amount of cards may be searched for, and two values may be exmpted from the search.
    ///</summary>
    ///<returns>An array of the indeces representing the highest valued cards in the search.</returns>
    ///<param name="numCardsNeeded">The amount of cards that are being searched for.</param>
    ///<param name="exemptRank1">The first rank that will be excluded from the search. May be left empty.</param>
    ///<param name="exemptRank2">The second rank that will be excluded from the search. May be left empty</param>
    private int[] CompleteHand(int numCardsNeeded, int exemptRank1 = -1, int execptRank2 = -1)
    {
        ulong cardBase = 1;
        cardBase <<= 63;
        ulong cardMask;
        int[] cards = new int[numCardsNeeded];
        int index = 0;

        for (int rank = 12; rank >= 0; --rank)
        {
            if (rank == exemptRank1 || rank == execptRank2)
                continue;

            cardMask = cardBase;
            for (int suit = 3; suit >= 0; --suit)
            {
                //If the card being investigated is in the contestant's hand
                if ((_binaryHand & cardMask) == cardMask)
                {
                    //Then add it to the array and move onto the next rank
                    cards[index] = suit * 13 + rank;
                    ++index;
                    if (index >= numCardsNeeded - 1)
                    {
                        return cards;
                    }
                    break;
                }
                cardMask >>= 13;
            }
            cardBase >>= 1;
        }

        return null;
    }
}
