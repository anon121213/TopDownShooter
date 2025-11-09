using System;
using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Managing;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.ArenaScene
{
  public class ArenaSceneEntryPoint : IInitializable, IDisposable
  {
    private readonly ArenaSceneScope _arenaScope;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IReadOnlyNetworkRoomModel _networkRoomModel;
    private readonly IPlayerSpawner _playerSpawner;
    private readonly IArenaSceneModel _arenaSceneModel;
    private readonly IEnemySpawnerModel _enemySpawnerModel;
    private readonly INetworkPlayerFactory _playerFactory;
    private readonly NetworkManager _networkManager;

    private readonly CompositeDisposable _disposables = new();

    private NetworkConfig _networkConfig;
    
    public ArenaSceneEntryPoint(ArenaSceneScope arenaScope,
      IStaticDataProvider staticDataProvider,
      IReadOnlyNetworkRoomModel networkRoomModel,
      IPlayerSpawner playerSpawner,
      IArenaSceneModel arenaSceneModel,
      IEnemySpawnerModel enemySpawnerModel,
      INetworkPlayerFactory playerFactory,
      NetworkManager networkManager)
    {
      _arenaScope = arenaScope;
      _staticDataProvider = staticDataProvider;
      _networkRoomModel = networkRoomModel;
      _playerSpawner = playerSpawner;
      _arenaSceneModel = arenaSceneModel;
      _enemySpawnerModel = enemySpawnerModel;
      _playerFactory = playerFactory;
      _networkManager = networkManager;
    }

    public void Initialize()
    {
      _networkConfig = _staticDataProvider.GetConfig<NetworkConfig>();
      
      if (!_networkRoomModel.IsServer.Value)
        return;
      
      // ------------PLAYERS-------------
      
      foreach (var player in _networkRoomModel.PlayersDto) 
        CreatePlayer(player.Value);
      
      _networkRoomModel.PlayersDto
        .ObserveAdd()
        .Subscribe(player => CreatePlayer(player.Value))
        .AddTo(_disposables);

      _networkRoomModel.Players
        .ObserveRemove()
        .Subscribe(player =>
        {
          if (_arenaSceneModel.LocalPlayersScopes.TryGetValue(player.Key, out var localScope)) 
            localScope.Dispose();
          
          if (_arenaSceneModel.RemotePlayersScopes.TryGetValue(player.Key, out var remoteScope)) 
            remoteScope.Dispose();
        })
        .AddTo(_disposables);

      _networkRoomModel.Clients
        .ObserveAdd()
        .Subscribe(client => _playerSpawner.SpawnPlayer(client.Value))
        .AddTo(_disposables);
      
      _networkRoomModel.Clients
        .ObserveRemove()
        .Subscribe(client => _playerSpawner.DespawnPlayer(client.Value))
        .AddTo(_disposables);
     
      _playerSpawner.SpawnPlayer(_networkRoomModel.ClientId.Value);
      
      // --------------ENEMIES--------------

      foreach (var mob in _networkRoomModel.MobsDto) 
        CreateMob(mob.Value); 

      _networkRoomModel.MobsDto
        .ObserveAdd()
        .Subscribe(mob => CreateMob(mob.Value))
        .AddTo(_disposables);

      _networkRoomModel.MobsDto
        .ObserveRemove()
        .Subscribe(enemy => _arenaSceneModel.MobScopes[enemy.Key].Dispose())
        .AddTo(_disposables);
      
      _enemySpawnerModel.StartSpawnEnemies();
    }

    // ------------PLAYERS--------------
    
    private void CreatePlayer(PlayerModelDTO model)
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
      
      _playerFactory.CreateNetworkPlayer(Vector3.zero, Quaternion.identity, model);
    }

    // -----------MOBS------------
    private void CreateMob(MobModelDataDTO dto)
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
        
      _arenaScope.CreateChildFromPrefab(_networkConfig.LocalMobScopePrefab, builder =>
        builder.RegisterInstance(dto));
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }
  }
}