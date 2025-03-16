using _Scripts.Gameplay.Collectables.Spawner;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Gameplay.Projectiles.Spawner;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.Installers
{
  public class SpawnersInstaller : MonoInstaller
  {
    [SerializeField] private EnemySpawner _enemySpawner;
    
    public override void Register(IContainerBuilder builder)
    {
      builder.Register<IProjectileSpawner, ProjectileSpawner>(Lifetime.Singleton);
      builder.Register<IPlayerSpawner, PlayerSpawner>(Lifetime.Singleton);
      builder.Register<ICollectableSpawner, CollectableSpawner>(Lifetime.Singleton);
      builder.RegisterInstance<IEnemySpawner>(_enemySpawner);
    }
  }
}