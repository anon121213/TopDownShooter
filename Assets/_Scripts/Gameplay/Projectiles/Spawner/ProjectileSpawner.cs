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
    private readonly IActorNumberAllocator _actorNumberAllocator;
    private readonly IProjectileFactory _projectileFactory;
    private readonly CompositeDisposable _disposables = new();
    
    public ProjectileSpawner(INetworkRoomModel networkRoomModel,
      IActorNumberAllocator actorNumberAllocator,
      IProjectileFactory projectileFactory)
    {
      _networkRoomModel = networkRoomModel;
      _actorNumberAllocator = actorNumberAllocator;
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
          Debug.LogError(dto.Key);
          var projectile = _projectileFactory.CreateProjectile(dto.Value);
          projectile.OnReturnToPool += DespawnProjectile;
        })
        .AddTo(_disposables);
      
      _networkRoomModel.ProjectilesDto
        .ObserveRemove()
        .Subscribe(dto =>
          _projectileFactory.ReturnToPool(dto.Value.ActorNumber, dto.Value.ProjectileType))
        .AddTo(_disposables);
    }

    public void SpawnProjectile(ProjectileTypeEnum projectileType, Vector3 position, Quaternion direction)
    {
      var actorNumber = _actorNumberAllocator.GetProjectileActorNumber();
      _networkRoomModel.AddProjectileDto(new ProjectileDataDTO(actorNumber, projectileType, position, direction.eulerAngles));
    }

    private void DespawnProjectile(Projectile projectile)
    {
      projectile.OnReturnToPool -= DespawnProjectile;
      _networkRoomModel.RemoveProjectileDto(projectile.ActorNumber);
    }
  }

  public interface IProjectileSpawner
  {
    void SpawnProjectile(ProjectileTypeEnum projectileType, Vector3 position, Quaternion direction);
  }
}