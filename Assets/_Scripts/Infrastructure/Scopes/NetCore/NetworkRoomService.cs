using System;
using _Scripts.Infrastructure.Scopes.Game;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomService : IInitializable, IDisposable
  {
    private readonly NetworkRoomScope _networkScope;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly NetworkManager _networkManager;
    private readonly GameScope _gameScope;
    private readonly NetworkRoomModel _networkRoomModelPrefab;

    private NetworkRoomModel _networkRoomModel;
    private IDisposable _disposable;
    
    public NetworkRoomService(NetworkRoomScope networkScope,
      IStaticDataProvider staticDataProvider,
      NetworkManager networkManager,
      GameScope gameScope,
      NetworkRoomModel networkRoomModelPrefab)
    {
      _networkScope = networkScope;
      _staticDataProvider = staticDataProvider;
      _networkManager = networkManager;
      _gameScope = gameScope;
      _networkRoomModelPrefab = networkRoomModelPrefab;
    }

    public void Initialize()
    {
#if UNITY_EDITOR
      var config = _staticDataProvider.GetConfig<NetworkConfig>();

      switch (config.ConnectType)
      {
        case ConnectType.Host:
          StartHost();
          break;
        case ConnectType.Client:
          ConnectToServer();
          break;
        default:
          Debug.LogError($"State by connect type {config.ConnectType} does not exist");
          break;
      }
#endif
    }

    private void StartHost()
    {
      _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
      _networkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
      _networkManager.ServerManager.StartConnection();
      _networkManager.ClientManager.StartConnection();

    }

    private void ConnectToServer()
    {
      _networkManager.ClientManager.OnAuthenticated += OnClientOnAuthenticated;
      _networkManager.ClientManager.StartConnection();
    }

    private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs state)
    {
      switch (state.ConnectionState)
      {
        case RemoteConnectionState.Started:
          _networkRoomModel.AddClient(connection.ClientId);
          break;

        case RemoteConnectionState.Stopped:
          _networkRoomModel.RemoveClient(connection.ClientId);
          break;
      }
    }

    private void OnServerConnectionState(ServerConnectionStateArgs obj)
    {
      if (obj.ConnectionState != LocalConnectionState.Started) 
        return;

      _networkRoomModel = Object.Instantiate(_networkRoomModelPrefab);
      _networkManager.ServerManager.Spawn(_networkRoomModel);

      _networkRoomModel.SetConnectionState(obj.ConnectionState);
      _networkRoomModel.SetIsServer(true);
      _networkRoomModel.SetId(0);
      _networkScope.CreateChildFromPrefab(_gameScope, builder => 
        builder.RegisterInstance(_networkRoomModel).AsImplementedInterfaces());
    }

    private void OnClientOnAuthenticated()
    {
      _networkManager.ClientManager.OnAuthenticated -= OnClientOnAuthenticated;
      _disposable = Observable.EveryUpdate().TakeWhile(_ => _networkRoomModel == null).Subscribe(_ =>
      {
        _networkRoomModel = Object.FindAnyObjectByType<NetworkRoomModel>(FindObjectsInactive.Include);

        if (!_networkRoomModel)
          return;
        
        _networkRoomModel.SetIsServer(false);
        _networkRoomModel.SetId(_networkManager.ClientManager.Connection.ClientId);
        _networkScope.CreateChildFromPrefab(_gameScope, builder => 
          builder.RegisterInstance(_networkRoomModel).AsImplementedInterfaces());
      });
    }

    public void Dispose()
    {
      _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
      _networkManager.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
      _disposable?.Dispose();
    }
  }
}