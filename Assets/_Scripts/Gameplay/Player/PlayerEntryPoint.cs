using System;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Managing;
using UniRx;
using UnityEngine;
using IInitializable = VContainer.Unity.IInitializable;

namespace _Scripts.Gameplay.Player
{
  public class PlayerEntryPoint : IInitializable, IDisposable
  {
    private readonly IPlayerServices _services;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly NetworkManager _networkManager;
    private readonly IPlayerFactory _playerFactory;
    private readonly IPlayerServices _playerServices;
    private readonly PlayerStateDTO _playerStateDto;
    private readonly IArenaSceneModel _arenaSceneModel;
    private readonly PlayerScope _playerScope;
    private readonly CompositeDisposable _disposables = new();
    
    private PlayerRootView _player;

    public PlayerEntryPoint(IPlayerServices services, INetworkRoomModel networkRoomModel,
      NetworkManager networkManager,IStaticDataProvider staticDataProvider, IPlayerFactory playerFactory,
      IPlayerServices playerServices, PlayerStateDTO playerStateDto, IArenaSceneModel arenaSceneModel,
      PlayerScope playerScope)
    {
      _services = services;
      _networkRoomModel = networkRoomModel;
      _staticDataProvider = staticDataProvider;
      _networkManager = networkManager;
      _playerFactory = playerFactory;
      _playerServices = playerServices;
      _playerStateDto = playerStateDto;
      _arenaSceneModel = arenaSceneModel;
      _playerScope = playerScope;
    }

    public void Initialize()
    {
      _player = _playerFactory.CreatePlayer(Vector3.zero, Quaternion.identity);
      
      if (_networkRoomModel.IsServer.Value) 
        _networkManager.ServerManager.Spawn(_player.gameObject);
      
      _player.PlayerModel.Apply(_playerStateDto);
      _networkRoomModel.AddPlayerLocal(_player.PlayerModel);
      _arenaSceneModel.AddPlayer(_player);
      _arenaSceneModel.AddPlayerScope(_player.PlayerModel.ActorNumber.Value, _playerScope);

      var config = _staticDataProvider.GetConfig<PlayerConfig>();
      _player.PlayerModel.SetConfig(config);

      if (_networkRoomModel.IsServer.Value)
        _player.PlayerModel.Health.Subscribe(health =>
        {
          if (health <= 0)
          {
            _player.PlayerModel.SetIsDead(true);
            return;
          }

          if (_player.PlayerModel.IsDead.Value && health > 0)
            _player.PlayerModel.SetIsDead(false);
        }).AddTo(_disposables);

      _player.PlayerModel.IsDead
        .Subscribe(isDead =>
        { if (isDead) Die(); })
        .AddTo(_disposables);
      
      _playerServices.ConstructServices(_player);
      _playerServices.InitializeServices();
      _playerServices.EnableServices();
    }

    private void Die()
    {
      _services.DisableServices();
      Debug.LogError("YouDied");
    }

    public void Dispose()
    {
      _playerFactory.ReturnPlayer(_player);
      _services.DisableServices();
      _arenaSceneModel.RemovePlayer(_player.PlayerModel.ActorNumber.Value);
      _arenaSceneModel.RemovePlayerScope(_player.PlayerModel.ActorNumber.Value);
      _disposables.Dispose();
    }
  }
}