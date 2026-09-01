namespace FrogBattleV4.Core.Entities;

public abstract class GameEntity
{
    private static long _idCounter;

    protected GameEntity()
    {
        Id = ++_idCounter;
    }

    public EntityId Id { get; }

    public override string ToString() => $"#{Id}";
    public override int GetHashCode() => Id.GetHashCode();
    public override bool Equals(object? obj) => obj is GameEntity entity && Id == entity.Id;
}