using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandScorer
{
    private const float ACE_KICKER = 0.0013f;
    private const float KING_KICKER = 0.0012f;
    private const float QUEEN_KICKER = 0.0011f;
    private const float JACK_KICKER = 0.0010f;
    private const float MAX_SCORE = 10000f;
    private const float MAX_RANK_SCORE = 1320981f;
    private const float MIN_RANK_SCORE = 100000f;
    private HandAnalyzer Analyzer;

    #region

    public HandScorer (HandAnalyzer ha)
    {
        Analyzer = ha;
    }

    //Return the score and the name of the hand
    public HandInfo ScoreHand(Hand hand)
    {
        (int[] bestHand, HandName name) = Analyzer.DetermineHand(hand);
        int rank;
        float score;
        string handTitle;
        //Now we know what the best hand the contestant has is, as well as the cards that comprise it.
        //Now, it just needs to be scored to be compared to other contestant's hands
        switch (name)
        {
            case HandName.RoyalFlush:
                return (HasRoyalFlush(), "Royal Flush", bestHand);

            case HandName.StraightFlush:
                return (HasStraightFlush(bestHand), "Straight Flush", bestHand);

            case HandName.FourOfAKind:
                return (HasFourOfAKind(bestHand), "Four of a Kind", bestHand);

            case HandName.FullHouse:
                return (HasFullHouse(bestHand), "Full House", bestHand);

            case HandName.Flush:
                return (HasFlush(bestHand), "Flush", bestHand);

            case HandName.Straight:
                return (HasStraight(bestHand), "Straight", bestHand);

            case HandName.ThreeOfAKind:
                return (HasThreeOfAKind(bestHand), "Three of a Kind", bestHand);

            case HandName.TwoPair:
                return (HasTwoPair(bestHand), "Two Pair", bestHand);

            case HandName.Pair:
                rank = bestHand[0] % 13;
                score = HasPair(bestHand);
                switch (rank)
                {
                    case 0:
                        handTitle = "Pair of Twos";
                        break;
                    case 1:
                        handTitle = "Pair of Threes";
                        break;
                    case 2:
                        handTitle = "Pair of Fours";
                        break;
                    case 3:
                        handTitle = "Pair of Fives";
                        break;
                    case 4:
                        handTitle = "Pair of Sixes";
                        break;
                    case 5:
                        handTitle = "Pair of Sevens";
                        break;
                    case 6:
                        handTitle = "Pair of Eights";
                        break;
                    case 7:
                        handTitle = "Pair of Nines";
                        break;
                    case 8:
                        handTitle = "Pair of Tens";
                        break;
                    case 9:
                        handTitle = "Pair of Jacks";
                        break;
                    case 10:
                        handTitle = "Pair of Queens";
                        break;
                    case 11:
                        handTitle = "Pair of Kings";
                        break;
                    case 12:
                        handTitle = "Pair of Aces";
                        break;
                    default:
                        handTitle = "Someting went wrong in pair";
                        break;
                }

                return (score, handTitle, bestHand);

            case HandName.High:
                rank = bestHand[0] % 13;
                score = CardRankScore(bestHand, false);
                switch (rank)
                {
                    case 0:
                        handTitle = "Two High";
                        break;
                    case 1:
                        handTitle = "Three High";
                        break;
                    case 2:
                        handTitle = "Four High";
                        break;
                    case 3:
                        handTitle = "Five High";
                        break;
                    case 4:
                        handTitle = "Six High";
                        break;
                    case 5:
                        handTitle = "Seven High";
                        break;
                    case 6:
                        handTitle = "Eight High";
                        break;
                    case 7:
                        handTitle = "Nine High";
                        break;
                    case 8:
                        handTitle = "Ten High";
                        break;
                    case 9:
                        handTitle = "Jack High";
                        break;
                    case 10:
                        handTitle = "Queen High";
                        break;
                    case 11:
                        handTitle = "King High";
                        break;
                    case 12:
                        handTitle = "Ace High";
                        break;
                    default:
                        handTitle = "Something went wrong in high";
                        break;
                }
                return (score, handTitle, bestHand);

            default:
                break;
        }
        return (0, null, null);
    }

    //Return 900
    public float HasRoyalFlush()
    {
        return 900;
    }

    //Return [800, 900)
    public float HasStraightFlush(int[] bestHand)
    {
        //The last card in the array is the highest rank
        float highestRank = bestHand[4] % 13;
        return 800 + (highestRank / 12) * 99;
    }

    //Return [700, 800)
    public float HasFourOfAKind(int[] bestHand)
    {
        //The first 4 cards in the array comprise the four of a kind
        float quadRank = bestHand[0] % 13;
        //The fifth card is a high card
        float highRank = bestHand[4] % 13;
        //Add the highranking card to the score past the decimal place
        return 700 + (quadRank / 12) * 99 + (highRank / 100);
    }

    //Retuen [600, 700)
    public float HasFullHouse(int[] bestHand)
    {
        //The first three cards comprise the three of a kind, and the latter 2 cards comprise the pair
        int trioRank = bestHand[0];
        int pairRank = bestHand[3];
        return 600 + FullHouseScore(trioRank, pairRank);

        float FullHouseScore(int threeRank, int twoRank)
        {
            //Max score consists of 3 Aces and 2 Kings (12 * 100) + (11 * 10) + 1, to make sure the returned result is less than 100
            //Add one to the end to ensure that the returned score does not reach 100
            float maxScore = 1311;
            float score = 0;
            //Make sure that first is given greater weight in the score than second
            score += threeRank * 100;
            score += twoRank * 10;
            return (score / maxScore) * 100;
        }
    }

    //Return [500, 600)
    public float HasFlush(int[] bestHand)
    {
        //Adjust score to take into account all the ranks in the flush
        return 500 + CardRankScore(bestHand, false);
    }

    //Return [400, 500)
    public float HasStraight(int[] bestHand)
    {
        //The last card is the highest rank
        float highRank = bestHand[4] % 13;
        return 400 + (highRank / 12) * 99;
    }

    //Return [300, 400)
    public float HasThreeOfAKind(int[] bestHand)
    {
        //The first three cards in the array comprise the three of a kind
        float trioRank = bestHand[0] % 13;
        return 300 + (trioRank / 12) * 99 + CardRankScore(bestHand, true, trioRank);
    }

    //Return [200, 300)
    public float HasTwoPair(int[] bestHand)
    {
        //The rank of the first pair is given by the first 2 cards. The rank of the second pair is given by the second 2 cards.
        //The greater of the two pairs is the first.
        int firstPair = bestHand[0] % 13;
        int secondPair = bestHand[2] % 13;
        //The rank of the high card is in the last index
        float highRank = bestHand[3] % 13;

        return 200 + TwoPairScore(firstPair, secondPair) + (highRank / 100);

        //CONDITION: First > Second
        float TwoPairScore(int first, int second)
        {
            //Max score consists of a two pair of Aces and Kings (12 * 100) + (11 * 10) + 1
            //Add one to the end to ensure that the returned score does not reach 100
            float maxScore = 1311;
            float score = 0;
            //Make sure that first is given greater weight in the score than second
            score += first * 100;
            score += second * 10;
            return (score / maxScore) * 100;
        }
    }

    //Return [100, 200)
    public float HasPair(int[] bestHand)
    {
        float pairRank = bestHand[0] % 13;
        return 100 + ((pairRank / 12) * 99) + CardRankScore(bestHand, true, pairRank);
    }


    //Return a score that takes the rank of each card into account, ignoring hands
    //PRECONDITIONL Hand must be sorted
    //Return [0, 100)
    //Ranks are from 0-12
    public float CardRankScore(int[] bestHand, bool append, float firstRankExempt = -1, float secondRankExempt = -1)
    {
        //Create array of 5 card ranks found in the hand, sorted greatest to least
        //If not all indices are filled, then place 0 into the empty indeces
        //Convert this into an integer
        int[] cardRanks = new int[5];
        //Go through every cardRank
        //Check if that rank exists in the hand
        //If it does, add it to the array
        float score = 0;

        int i = 0;
        while (i < 5)
        {
            if (bestHand[i] == firstRankExempt || bestHand[i] == secondRankExempt)
            {
                cardRanks[i] = 0;
                ++i;
            }
            else
            {
                cardRanks[i] = bestHand[i] % 13;
                ++i;
            }
        }

        Array.Sort(cardRanks);
        Array.Reverse(cardRanks);

        //DOnt take cards straight from the array, take their ranks instreasd
        for (i = 0; i < cardRanks.Length; ++i)
        {
            score += cardRanks[i] * (float) Math.Pow(10, cardRanks.Length - i);
        }

        //If append is true, then score is returned in the range of [0, 1)
        if (append)
            return score / MAX_RANK_SCORE;
        //Else, return the score in the range of [0, 100)
        else
            return (score / MAX_RANK_SCORE) * 100;
    }
    #endregion
}
