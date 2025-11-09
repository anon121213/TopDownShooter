using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Scopes.NetCore.Data;
using _Scripts.Infrastructure.Services.Network;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Projectiles.Spawner
{
  public class ProjectileSpawner : IProjectileSpawner, IInitializable
  {
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly IProjectileFactory _projectileFactory;
    private readonly CompositeDisposable _disposables = new();
    
    public ProjectileSpawner(INetworkRoomModel networkRoomModel,
      IProjectileFactory projectileFactory)
    {
      _networkRoomModel = networkRoomModel;
      _projectileFactory = projectileFactory;
    }

    public void Initialize()
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
      
      _networkRoomModel.ProjectilesDto
        .ObserveAdd()
        .Subscribe(dto =>
        {
          var projectile = _projectileFactory.CreateProjectile(dto.Value);
          projectile.OnReturnToPool += DespawnProjectile;
        })
        .AddTo(_disposables);
    }

    public void SpawnProjectile(ProjectileTypeEnum projectileType, Vector3 position, Quaternion direction) => 
      _networkRoomModel.AddProjectileDto(new ProjectileDataDTO(-1, projectileType, position, direction.eulerAngles));

    private void DespawnProjectile(Projectile projectile)
    {
      _networkRoomModel.RemoveProjectileDto(projectile.ActorNumber);
      projectile.OnReturnToPool -= DespawnProjectile;
      _projectileFactory.ReturnToPool(projectile);
    }
  }

  public interface IProjectileSpawner
  {
    void SpawnProjectile(ProjectileTypeEnum projectileType, Vector3 position, Quaternion direction);
  }
}