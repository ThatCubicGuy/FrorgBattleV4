using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityExecContext
{
    [NotNull] public required IBattleMember User { get; init; }
    [NotNull] public required AbilityDefinition Definition { get; init; }

    /// <summary>
    /// Main target selected by the user.
    /// </summary>
    [NotNull]
    public required IBattleMember MainTarget { get; init; }

    /// <summary>
    /// Pool of targets that the ability's targeting components can select from, knowing the main target.
    /// </summary>
    /// <remarks>Order sensitive.</remarks>
    [NotNull]
    public required IEnumerable<IBattleMember> ValidTargets { get; init; }

    [NotNull] public required Random Rng { get; init; }
}

public static class AbilityExecContextExtensions
{
    [Pure]
    public static ModifierContext AsModCtx(this AbilityExecContext ctx)
    {
        return new ModifierContext
        {
            Ability = ctx.Definition,
            Actor = ctx.User,
            Other = ctx.MainTarget,
            Rng = ctx.Rng,
        };
    }
}