#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Contexts;

/// <summary>
/// Represents a context that has a "reference", an "other" battle member that we are
/// using to calculate whatever it is we need in relation to that specific member.
/// </summary>
public interface IReferenceContext
{
    BattleMember? Other { get; }
}