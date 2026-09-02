using System.Collections.Generic;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core.Combat;

public class Team : Identifiable<TeamUid>
{
    public Team(ISelectionProvider playerSelectionProvider, IEnumerable<IModifierProvider>? modifiers = null)
    {
        System.ArgumentNullException.ThrowIfNull(playerSelectionProvider);
        Provider = playerSelectionProvider;
        Modifiers = modifiers is not null ? new ModifierCollection(modifiers) : ModifierCollection.Empty;
    }

    public ISelectionProvider Provider { get; }
    public ModifierCollection Modifiers { get; }
}