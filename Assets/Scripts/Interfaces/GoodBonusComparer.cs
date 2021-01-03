using System.Collections.Generic;


namespace Labyrinth
{
    public sealed class GoodBonusComparer : IComparer<GoodBonus>
    {
        public int Compare (GoodBonus x, GoodBonus y)
        {
            if (x.point < y.point) 
                return 1;

            if (x.point > y.point) 
                return -1;

            return 0;
        }
    }
}
