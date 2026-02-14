#nullable enable
using System.Collections;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public class BattleMemberParts : IEnumerable
{
    private readonly ITargetable?[,] _parts = new ITargetable?[8, 2 + DepthOffset];
    public const int DepthOffset = 2;

    public ITargetable? this[TargetTag tag]
    {
        get
        {
            foreach (var part in _parts)
            {
                if (part?.Tags.Contains(tag) ?? false) return part;
            }

            return null;
        }
    }

    public ITargetable? this[TargetPosition position]
    {
        get => this[position.Height, position.Depth];
        init => this[position.Height, position.Depth] = value;
    }

    private ITargetable? this[int height, int depth]
    {
        // Use a sort of layering system for depth -
        // everything by default is zero and then
        // if you want something behind most things,
        // depth = 1, but if you want something
        // in front of most things, depth = -1
        // we shall support 4 layers for now,
        // with layer DepthOffset as the default
        get => _parts[height, depth + DepthOffset];
        init => _parts[height, depth + DepthOffset] = value;
    }

    public int Height => _parts.GetLength(0);
    public int Depth => _parts.GetLength(1) - DepthOffset;
    IEnumerator IEnumerable.GetEnumerator() => _parts.GetEnumerator();
}