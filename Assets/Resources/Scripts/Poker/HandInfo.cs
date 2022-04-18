using System;

    public struct HandInfo
    {
        public static readonly HandInfo Empty = new HandInfo { Cards = null, HandName = "", HandScore = 0f};
        public string HandName;
        public int[] Cards;
        public float HandScore;
    }

