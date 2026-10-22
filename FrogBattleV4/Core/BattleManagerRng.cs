using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core;

public partial class BattleManager
{
    private class SeededBattleRng(int seed) : IBattleRng
    {
        private readonly Random _random = new(seed);

        public bool Chance(double chance)
        {
            return chance > _random.NextDouble();
        }

        public T Choose<T>(IEnumerable<T> values)
        {
            return values.MinBy(_ => _random.Next())
                   ?? throw new InvalidOperationException("Values cannot be an empty collection.");
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            return source.OrderBy(_ => _random.Next());
        }
    }
}