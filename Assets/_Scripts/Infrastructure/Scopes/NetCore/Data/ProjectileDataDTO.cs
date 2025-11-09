using _Scripts.Gameplay.Projectiles.Data;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.NetCore.Data
{
  public readonly struct ProjectileDataDTO
  {
    public readonly int ActorNumber;
    public readonly ProjectileTypeEnum ProjectileType;
    public readonly Vector3 Position;
    public readonly Vector3 Rotation;

    public ProjectileDataDTO(int actorNumber, ProjectileTypeEnum projectileType, Vector3 position, Vector3 rotation)
    {
      ActorNumber = actorNumber;
      ProjectileType = projectileType;
      Position = position;
      Rotation = rotation;
    }

    public ProjectileDataDTO CloneAndSetActorNumber(int actorNumber) => 
      new(actorNumber, ProjectileType, Position, Rotation);
  }
}