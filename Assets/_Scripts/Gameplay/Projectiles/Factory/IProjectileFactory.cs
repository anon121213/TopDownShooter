using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Infrastructure.Scopes.NetCore.Data;

namespace _Scripts.Gameplay.Projectiles.Factory
{
  public interface IProjectileFactory
  {
    Projectile CreateProjectile(ProjectileDataDTO projectileDataDto);
    void ReturnToPool(int actorNumber, ProjectileTypeEnum type);
  }
}