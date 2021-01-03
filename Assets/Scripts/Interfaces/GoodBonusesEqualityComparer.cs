using System.Collections.Generic;


namespace Labyrinth
{
    public sealed class GoodBonusesEqualityComparer : IEqualityComparer<GoodBonus>
    {
        public bool Equals(GoodBonus x, GoodBonus y) => x.point == y.point;

        public int GetHashCode(GoodBonus obj) => obj.point.GetHashCode();
    }
}
