using System;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UniRx;
using UnityEngine;
using IInitializable = VContainer.Unity.IInitializable;

namespace _Scripts.Gameplay.Player
{
  public class LocalPlayerEntryPoint : IInitializable, IDisposable
  {
    private readonly IPlayerServices _services;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IPlayerFactory _playerFactory;
    private readonly IPlayerServices _playerServices;
    private readonly IArenaSceneModel _arenaSceneModel;
    private readonly LocalPlayerScope _localPlayerScope;
    private readonly NetworkPlayerView _networkPlayer;
    private readonly CompositeDisposable _disposables = new();

    private LocalPlayerView _localPlayer;

    public LocalPlayerEntryPoint(IPlayerServices services, INetworkRoomModel networkRoomModel,
      IStaticDataProvider staticDataProvider, IPlayerFactory playerFactory,
      IPlayerServices playerServices, IArenaSceneModel arenaSceneModel,
      LocalPlayerScope localPlayerScope, NetworkPlayerView networkPlayer)
    {
      _services = services;
      _networkRoomModel = networkRoomModel;
      _staticDataProvider = staticDataProvider;
      _playerFactory = playerFactory;
      _playerServices = playerServices;
      _arenaSceneModel = arenaSceneModel;
      _localPlayerScope = localPlayerScope;
      _networkPlayer = networkPlayer;
    }

    public void Initialize()
    {
      _localPlayer = _playerFactory.CreateLocalPlayer(Vector3.zero, Quaternion.identity, _networkPlayer.transform);

      var config = _staticDataProvider.GetConfig<PlayerConfig>();
      _networkPlayer.PlayerModel.SetConfig(config);
      
      _networkPlayer.PlayerModel.IsSynced.Subscribe(isSynced =>
      {
        if(!isSynced)
          return;
        
        _networkRoomModel.AddPlayer(_networkPlayer.PlayerModel);
        _arenaSceneModel.AddPlayer(_networkPlayer);  
        _arenaSceneModel.AddLocalPlayerScope(_networkPlayer.PlayerModel.ActorNumber.Value, _localPlayerScope);
        
        _networkPlayer.PlayerModel.IsDead
          .Subscribe(isDead => { if (isDead) Die(); })
          .AddTo(_disposables);

        _playerServices.ConstructServices(_networkPlayer, _localPlayer);
        _playerServices.InitializeServices();
        _playerServices.EnableServices();
      }).AddTo(_disposables);
    }

    private void Die()
    {
      _services.DisableServices();
      Debug.LogError("YouDied");
    }

    public void Dispose()
    {
      _services.DisableServices();
      _arenaSceneModel.RemovePlayer(_networkPlayer.PlayerModel.ActorNumber.Value);
      _arenaSceneModel.RemoveLocalPlayerScope(_networkPlayer.PlayerModel.ActorNumber.Value);
      _disposables.Dispose();
    }
  }
}