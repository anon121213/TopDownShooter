using System;
using System.Collections.Generic;
using System.Threading;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Infrastructure.Scopes.NetCore.Data;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Network;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Projectiles.Factory
{
  public class ProjectileFactory : IProjectileFactory, IInitializable, IWarmupable
  {
    private readonly Dictionary<int, Projectile> _projectiles = new();
    private readonly IObjectPool _objectPool;
    private readonly NetworkManager _networkManager;
    private readonly INetworkDamageService _networkDamageService;
    private readonly IStaticDataProvider _staticDataProvider;
    private ProjectilesConfig _projectileConfig;

    public ProjectileFactory(IObjectPool objectPool,
      NetworkManager networkManager,
      INetworkDamageService networkDamageService,
      IStaticDataProvider staticDataProvider)
    {
      _objectPool = objectPool;
      _networkManager = networkManager;
      _networkDamageService = networkDamageService;
      _staticDataProvider = staticDataProvider;
    }

    public void Initialize() => 
      _projectileConfig = _staticDataProvider.GetConfig<ProjectilesConfig>();

    public UniTask Warmup(CancellationToken ct)
    {
      foreach (var projectile in _projectileConfig.Projectiles) 
        _objectPool.Warmup(projectile.Prefab.gameObject);
      
      return UniTask.CompletedTask;
    }

    public Projectile CreateProjectile(ProjectileDataDTO projectileDataDto)
    {
      if (!_projectileConfig.TryGetProjectile(projectileDataDto.ProjectileType, out var projectileData))
        return null;
      
      var projectile = _objectPool.GetGameObject(projectileData.Prefab, projectileDataDto.Position, Quaternion.Euler(projectileDataDto.Rotation));
      _networkManager.ServerManager.Spawn(projectile.gameObject);
      _projectiles.Add(projectileDataDto.ActorNumber, projectile);
      projectile.SetActorNumber(projectileDataDto.ActorNumber);
      projectile.Construct(projectileData, _networkDamageService);
      projectile.Initialize();
      return projectile;
    }

    public void ReturnToPool(int actorNumber, ProjectileTypeEnum type)
    {
      if (!_projectiles.TryGetValue(actorNumber, out var projectile))
      {
        Debug.LogError($"Projectile with actor number {actorNumber} does not exist!!!");
        return;
      }

      _projectileConfig.TryGetProjectile(type, out var projectileData);
      _networkManager.ServerManager.Despawn(projectile.gameObject);
      _objectPool.ReturnGameObject(projectile.gameObject, projectileData.Prefab.gameObject);
      _projectiles.Remove(actorNumber);
    }
  }
}