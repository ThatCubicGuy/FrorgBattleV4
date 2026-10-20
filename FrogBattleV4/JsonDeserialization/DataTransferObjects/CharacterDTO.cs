using System.Text.Json.Serialization;

namespace FrogBattleV4.JsonDeserialization.DataTransferObjects;

[JsonSerializable(typeof(CharacterDto))]
public partial class CharacterDto : JsonSerializerContext
{
    // TODO (Nova)
}