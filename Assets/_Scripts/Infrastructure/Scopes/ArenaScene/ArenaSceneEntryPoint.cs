using System;
using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
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

    private readonly CompositeDisposable _disposables = new();

    private NetworkConfig _networkConfig;
    
    public ArenaSceneEntryPoint(ArenaSceneScope arenaScope,
      IStaticDataProvider staticDataProvider,
      IReadOnlyNetworkRoomModel networkRoomModel,
      IPlayerSpawner playerSpawner,
      IArenaSceneModel arenaSceneModel,
      IEnemySpawnerModel enemySpawnerModel)
    {
      _arenaScope = arenaScope;
      _staticDataProvider = staticDataProvider;
      _networkRoomModel = networkRoomModel;
      _playerSpawner = playerSpawner;
      _arenaSceneModel = arenaSceneModel;
      _enemySpawnerModel = enemySpawnerModel;
    }

    public void Initialize()
    {
      _networkConfig = _staticDataProvider.GetConfig<NetworkConfig>();
      
      // ------------PLAYERS-------------
      
      foreach (var player in _networkRoomModel.PlayersDto) 
        CreatePlayer(player.Value);
      
      _networkRoomModel.PlayersDto
        .ObserveAdd()
        .Subscribe(player => CreatePlayer(player.Value))
        .AddTo(_disposables);

      _networkRoomModel.PlayersDto
        .ObserveRemove()
        .Subscribe(player => _arenaSceneModel.PlayersScopes[player.Key].Dispose())
        .AddTo(_disposables);
      
      _playerSpawner.SpawnLocalPlayer();
      
      // --------------ENEMIES--------------

      foreach (var mob in _networkRoomModel.MobsDto) 
        CreateMob(mob.Value); 

      _networkRoomModel.MobsDto
        .ObserveAdd()
        .Subscribe(mob => CreateMob(mob.Value))
        .AddTo(_disposables);

      _networkRoomModel.MobsDto
        .ObserveRemove()
        .Subscribe(enemy => _arenaSceneModel.PlayersScopes[enemy.Key].Dispose())
        .AddTo(_disposables);
      
      _enemySpawnerModel.StartSpawnEnemies();
    }

    // ------------PLAYERS--------------
    
    private void CreatePlayer(PlayerStateDTO state)
    {
      if (_networkRoomModel.ClientId.Value == state.ActorNumber)
      {
        CreateLocalPlayer(state);
        return;
      }
          
      CreateRemotePlayer();
    }
    
    private void CreateLocalPlayer(PlayerStateDTO state)
    {
      _arenaScope.CreateChildFromPrefab(
        _networkConfig.PlayerScopePrefab,
        builder => builder.RegisterInstance(state)
      );
    }

    private void CreateRemotePlayer()
    {
      Debug.LogError("CreateRemotePlayer");
    }

    // -----------MOBS------------
    private void CreateMob(MobModelDataDTO dto)
    {
      if (_networkRoomModel.IsServer.Value)
      {
        CreateLocalMob(dto);
        return;
      }
        
      CreateRemoteMob(dto);
    }
    
    private void CreateLocalMob(MobModelDataDTO dto)
    {
      _arenaScope.CreateChildFromPrefab(_networkConfig.MobScopePrefab, builder =>
        builder.RegisterInstance(dto));
    }
    
    private void CreateRemoteMob(MobModelDataDTO dto)
    {
      Debug.LogError("CreateRemoteMob");
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }
  }
}