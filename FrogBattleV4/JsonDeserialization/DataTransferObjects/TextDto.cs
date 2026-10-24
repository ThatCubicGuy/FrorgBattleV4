using System.Text.Json;

namespace FrogBattleV4.JsonDeserialization.DataTransferObjects;

internal class TextDto
{
    public string? Text { get; init; }
    public string? Translate { get; init; }
    public TextDto[]? With { get; init; }
    public string? Fallback { get; init; }
    public TextDto? Extra { get; init; }
}