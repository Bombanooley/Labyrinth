using UnityEngine;


namespace Labyrinth
{
    public class MementoGameController : IMemento
    {
        public string name { get; private set; }
        public int seed { get; private set; }
        public int rows { get; private set; }
        public int columns { get; private set; }
        public int score { get; private set; }
        public float goodBonusRate { get; private set; }
        public float badBonusRate { get; private set; }
        public float deadBonusRate { get; private set; }
        public float boosterRate { get; private set; }

        public MementoGameController(string name, int seed, int rows, int columns, 
            int score, float goodBonusRate, float badBonusRate, float deadBonusRate, float boosterRate)
        {
            
            this.seed = seed;
            this.rows = rows;
            this.columns = columns;
            this.score = score;
            this.goodBonusRate = goodBonusRate;
            this.badBonusRate = badBonusRate;
            this.deadBonusRate = deadBonusRate;
            this.boosterRate = boosterRate;
        }
    }
}
