#nullable enable
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core;

public record ModifierContext<TRequest>(TRequest Request) : Contexts.IReferenceContext where TRequest : struct
{
    public BattleMember? Actor { get; init; }
    public BattleMember? Other { get; init; }
    public AbilityDefinition? Ability { get; init; }
    public IModifierProvider? ExtraModifiers { get; init; }
}

public record ModifierQuery<TRequest>(TRequest Request, ModifierDirection Direction) where TRequest : struct;

public static class ModifierResolver
{
    public static ModifierStack Resovle<TRequest>(this ModifierContext<TRequest> ctx) where TRequest : struct
    {
        
    }
}