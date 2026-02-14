#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.BattleSystem;

// TODO (to REDO) the whole targeting system... ugh
public interface ITargetable
{
    BattleMember Parent { get; }
    TargetIdentifier Identifier { get; }

    /// <summary>
    /// Modifiers for incoming damage.
    /// </summary>
    IEnumerable<DamageModifier> DamageModifiers { get; }
}

public class PartComparer : IEqualityComparer<ITargetable?>
{
    private readonly Func<ITargetable?, ITargetable?, bool> _equator;
    private readonly Func<ITargetable, int> _hasher;

    private PartComparer(Func<ITargetable?, ITargetable?, bool> equatorFunc, Func<ITargetable, int> hasherFunc)
    {
        _equator = equatorFunc;
        _hasher = hasherFunc;
    }

    bool IEqualityComparer<ITargetable?>.Equals(ITargetable? x, ITargetable? y) => _equator(x, y);
    int IEqualityComparer<ITargetable?>.GetHashCode(ITargetable obj) => _hasher(obj);

    // int IComparer<ITargetable>.Compare(ITargetable? x, ITargetable? y) => _equator(x, y) ? 0 : _comparer(x, y);
    // private static int CompareByHeightAndDepth(ITargetable? x, ITargetable? y)
    // {
    //     if (ReferenceEquals(x, y)) return 0;
    //     if (ReferenceEquals(null, y)) return 1;
    //     if (ReferenceEquals(null, x)) return -1;
    //     return x.Position.Height == y.Position.Height
    //         ? x.Position.Depth.CompareTo(y.Position.Depth)
    //         : x.Position.Height.CompareTo(y.Position.Height);
    // }

    private static bool EqualsByParentAndRole(ITargetable? x, ITargetable? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        return x.Parent.Equals(y.Parent) && x.Identifier.Equals(y.Identifier);
    }

    private static int GetHashCodeByParent(ITargetable obj)
    {
        return HashCode.Combine(obj.Parent);
    }

    public static readonly PartComparer DefaultComparer =
        new(EqualsByParentAndRole, GetHashCodeByParent);
}