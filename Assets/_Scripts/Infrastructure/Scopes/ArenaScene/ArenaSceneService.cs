using System;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.ArenaScene
{
  public class ArenaSceneService : IInitializable, IDisposable
  {
    private readonly LifetimeScope _arenaScope;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly IEnemySpawner _enemySpawner;
    private readonly IReadOnlyNetworkRoomModel _networkRoomModel;

    private readonly CompositeDisposable _disposables = new();

    public ArenaSceneService(LifetimeScope arenaScope,
      IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      IEnemySpawner enemySpawner,
      IReadOnlyNetworkRoomModel networkRoomModel)
    {
      _arenaScope = arenaScope;
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _enemySpawner = enemySpawner;
      _networkRoomModel = networkRoomModel;
    }

    public void Initialize()
    {
      Debug.LogError(_arenaScope.Parent.name);
      Debug.LogError(_arenaScope.Parent.Parent.name);
      Debug.LogError(_networkRoomModel.Clients.Count);
      Debug.LogError(_networkRoomModel.IsServer.Value);
      Debug.LogError(_networkRoomModel.InstanceTag);
      
      foreach (var client in _networkRoomModel.Clients)
      {
        if (_networkRoomModel.ClientId.Value == client)
        {
          CreateLocalPlayer();
          return;
        }
          
        CreateRemotePlayer();
      }
      
      _networkRoomModel.Clients
        .ObserveAdd()
        .Subscribe(clientId =>
        {
          if (_networkRoomModel.ClientId.Value == clientId.Value)
          {
            CreateLocalPlayer();
            return;
          }
          
          CreateRemotePlayer();
        })
        .AddTo(_disposables);

      CreateEnemy(); // TODO REMOVE AND MAKE ENEMY SCOPE
    }

    private void CreateLocalPlayer()
    {
      var config = _staticDataProvider.GetConfig<PlayerConfig>();
      _arenaScope.CreateChildFromPrefab(config.Prefab.PlayerScope);
    }

    private void CreateRemotePlayer()
    {
      Debug.LogError("CreateRemotePlayer");
    }

    private void CreateEnemy()
    {
      _enemySpawner.CreateSimpleEnemiesOnSpawnPoints();

      _assetProvider.Cleanup();
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }
  }
}