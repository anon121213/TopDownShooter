using System;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class RemotePlayerEntryPoint : IInitializable, IDisposable
  {
    private readonly NetworkPlayerView _networkPlayer;
    private readonly IRemotePlayerFactory _playerFactory;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly IArenaSceneModel _arenaSceneModel;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly RemotePlayerScope _remotePlayerScope;
    private RemotePlayerView _remotePlayer;

    private readonly CompositeDisposable _disposables = new();

    public RemotePlayerEntryPoint(NetworkPlayerView networkPlayer,
      IRemotePlayerFactory playerFactory,
      INetworkRoomModel networkRoomModel,
      IArenaSceneModel arenaSceneModel,
      IStaticDataProvider staticDataProvider,
      RemotePlayerScope remotePlayerScope)
    {
      _networkPlayer = networkPlayer;
      _playerFactory = playerFactory;
      _networkRoomModel = networkRoomModel;
      _arenaSceneModel = arenaSceneModel;
      _staticDataProvider = staticDataProvider;
      _remotePlayerScope = remotePlayerScope;
    }
    
    public void Initialize()
    {
      _remotePlayer = _playerFactory.CreateRemotePlayer(Vector3.zero, Quaternion.identity, _networkPlayer.transform);

      var config = _staticDataProvider.GetConfig<PlayerConfig>();
      _networkPlayer.PlayerModel.SetConfig(config);
      
      _networkPlayer.PlayerModel.IsSynced.Subscribe(isSynced =>
      {
        if (!isSynced)
          return;
        
        _networkRoomModel.AddPlayer(_networkPlayer.PlayerModel);
        _arenaSceneModel.AddPlayer(_networkPlayer);  
        _arenaSceneModel.AddRemotePlayerScope(_networkPlayer.PlayerModel.ActorNumber.Value, _remotePlayerScope);
      }).AddTo(_disposables);
    }

    public void Dispose()
    {
      _arenaSceneModel.RemovePlayer(_networkPlayer.PlayerModel.ActorNumber.Value);
      _arenaSceneModel.RemoveRemotePlayerScope(_networkPlayer.PlayerModel.ActorNumber.Value);
      _disposables?.Dispose();
    }
  }
}