using System;
using _Scripts.Infrastructure.NetCore;
using _Scripts.Infrastructure.Scopes.Game;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomService : IInitializable, IDisposable
  {
    private readonly NetworkRoomScope _networkScope;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly NetworkManager _networkManager;
    private readonly GameScope _gameScope;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly INetworkSyncService _syncService;

    public NetworkRoomService(NetworkRoomScope networkScope,
      IStaticDataProvider staticDataProvider,
      NetworkManager networkManager,
      GameScope gameScope,
      INetworkRoomModel networkRoomModel,
      INetworkSyncService syncService)
    {
      _networkScope = networkScope;
      _staticDataProvider = staticDataProvider;
      _networkManager = networkManager;
      _gameScope = gameScope;
      _networkRoomModel = networkRoomModel;
      _syncService = syncService;
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
      Debug.LogError(_networkRoomModel.InstanceTag);
    }

    private void StartHost()
    {
      _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
      _networkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
      _networkManager.ServerManager.StartConnection();
      _networkManager.ClientManager.StartConnection();
      _networkRoomModel.SetIsServer(true);
    }

    private void ConnectToServer()
    {
      _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
      _networkManager.ClientManager.StartConnection();
      _networkRoomModel.SetIsServer(false);
      _networkRoomModel.SetId(0);
    }

    private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs state)
    {
      switch (state.ConnectionState)
      {
        case RemoteConnectionState.Started:
          _syncService.AddClient(connection.ClientId);
          break;

        case RemoteConnectionState.Stopped:
          _syncService.RemoveClient(connection.ClientId);
          break;
      }
    }

    private void OnServerConnectionState(ServerConnectionStateArgs obj)
    {
      _networkRoomModel.SetConnectionState(obj.ConnectionState);

      if (obj.ConnectionState == LocalConnectionState.Started)
        _networkScope.CreateChildFromPrefab(_gameScope);
    }

    private void OnClientConnectionState(ClientConnectionStateArgs obj)
    {
      _networkRoomModel.SetConnectionState(obj.ConnectionState);

      if (obj.ConnectionState == LocalConnectionState.Started)
      {
        _networkRoomModel.SetId(_networkManager.ClientManager.Connection.ClientId);
        _networkScope.CreateChildFromPrefab(_gameScope);
      }
    }

    public void Dispose()
    {
      _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
      _networkManager.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
      _networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }
  }
}