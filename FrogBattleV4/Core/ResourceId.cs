namespace FrogBattleV4.Core;

/// <summary>
/// Contextual resource location indicating some manner of registry item.
/// </summary>
/// <param name="Namespace">Namespace of the resource ID; e.g. "frorgbattle".</param>
/// <param name="Path">Path of the resource ID; e.g. "rexulti".</param>
public readonly record struct ResourceId(string Namespace, string Path)
{
    public override string ToString() => $"{Namespace}:{Path}";
}