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
        Debug.Log("Scorer " + hand.cards.Count);
        (int[] bestHand, HandName name) = Analyzer.DetermineHand(hand);
        int rank;
        HandInfo info = HandInfo.Empty;
        info.Cards = bestHand;

        //Now we know what the best hand the contestant has is, as well as the cards that comprise it.
        //Now, it just needs to be scored to be compared to other contestant's hands
        switch (name)
        {
            case HandName.RoyalFlush:
                info.HandScore = HasRoyalFlush();
                info.HandName = "Royal Flush";
                break;

            case HandName.StraightFlush:
                info.HandScore = HasStraightFlush(bestHand);
                info.HandName = "Straight Flush";
                break;

            case HandName.FourOfAKind:
                info.HandScore = HasFourOfAKind(bestHand);
                info.HandName = "Four Of A Kind";
                break;

            case HandName.FullHouse:
                info.HandScore = HasFullHouse(bestHand);
                info.HandName = "Full House";
                break;

            case HandName.Flush:
                info.HandScore = HasFlush(bestHand);
                info.HandName = "Flush";
                break;

            case HandName.Straight:
                info.HandScore = HasFlush(bestHand);
                info.HandName = "Straight";
                break;

            case HandName.ThreeOfAKind:
                info.HandScore = HasThreeOfAKind(bestHand);
                info.HandName = "Three Of A Kind";
                break;

            case HandName.TwoPair:
                info.HandScore = HasTwoPair(bestHand);
                info.HandName = "Two Pair";
                break;

            case HandName.Pair:
                rank = bestHand[0] % 13;
                info.HandScore = HasPair(bestHand);
                switch (rank)
                {
                    case 0:
                        info.HandName = "Pair Of Twos";
                        break;
                    case 1:
                        info.HandName = "Pair Of Threes";
                        break;
                    case 2:
                        info.HandName = "Pair Of Fours";
                        break;
                    case 3:
                        info.HandName = "Pair Of Fives";
                        break;
                    case 4:
                        info.HandName = "Pair Of Sixes";
                        break;
                    case 5:
                        info.HandName = "Pair Of Sevens";
                        break;
                    case 6:
                        info.HandName = "Pair Of Eights";
                        break;
                    case 7:
                        info.HandName = "Pair Of Nines";
                        break;
                    case 8:
                        info.HandName = "Pair Of Tens";
                        break;
                    case 9:
                        info.HandName = "Pair Of Jacks";
                        break;
                    case 10:
                        info.HandName = "Pair Of Queens";
                        break;
                    case 11:
                        info.HandName = "Pair Of Kings";
                        break;
                    case 12:
                        info.HandName = "Pair Of Aces";
                        break;
                    default:
                        info.HandName = "Someting went wrong in pair";
                        break;
                }

                break;

            case HandName.High:
                rank = bestHand[0] % 13;
                info.HandScore = CardRankScore(bestHand, false);
                switch (rank)
                {
                    case 0:
                        info.HandName = "Two High";
                        break;
                    case 1:
                        info.HandName = "Three High";
                        break;
                    case 2:
                        info.HandName = "Four High";
                        break;
                    case 3:
                        info.HandName = "Five High";
                        break;
                    case 4:
                        info.HandName = "Six High";
                        break;
                    case 5:
                        info.HandName = "Seven High";
                        break;
                    case 6:
                        info.HandName = "Eight High";
                        break;
                    case 7:
                        info.HandName = "Nine High";
                        break;
                    case 8:
                        info.HandName = "Ten High";
                        break;
                    case 9:
                        info.HandName = "Jack High";
                        break;
                    case 10:
                        info.HandName = "Queen High";
                        break;
                    case 11:
                        info.HandName = "King High";
                        break;
                    case 12:
                        info.HandName = "Ace High";
                        break;
                    default:
                        info.HandName = "Something went wrong in high";
                        break;
                }
                break;

            default:
                info = HandInfo.Empty;
                break;
        }
        return info;
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
