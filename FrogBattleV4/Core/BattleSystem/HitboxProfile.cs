#nullable enable
using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public class HitboxProfile : IEnumerable<ITargetable?>
{
    /// <summary>
    /// The parts belonging to this battle member, ordered from closest to the ground.
    /// </summary>
    private readonly FrozenSet<ITargetable?> _partsByHeight;
    private readonly FrozenDictionary<TargetIdentifier, ITargetable?> _partsByIdentifier;

    /// <summary>
    /// Creates a new HitboxProfile to be used by a BattleMember.
    /// </summary>
    /// <param name="orderedParts">Parts ordered from lowest to highest. Null means empty space.</param>
    public HitboxProfile(params ITargetable?[] orderedParts)
    {
        _partsByHeight = orderedParts.ToFrozenSet(PartComparer.DefaultComparer);
    }

    public ITargetable? this[TargetIdentifier id] => _partsByIdentifier[id];

    public ITargetable? this[TargetPosition position] => this[position.Height];

    private ITargetable? this[int height] => _partsByHeight.Items[height];

    public int Height => _partsByHeight.Count;
    IEnumerator IEnumerable.GetEnumerator() => _partsByHeight.GetEnumerator();
    IEnumerator<ITargetable?> IEnumerable<ITargetable?>.GetEnumerator() => _partsByHeight.Cast<ITargetable?>().GetEnumerator();
}