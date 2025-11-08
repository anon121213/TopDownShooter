using System;
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
      
    public NetworkRoomService(NetworkRoomScope networkScope,
      IStaticDataProvider staticDataProvider,
      NetworkManager networkManager,
      GameScope gameScope,
      INetworkRoomModel networkRoomModel)
    {
      _networkScope = networkScope;
      _staticDataProvider = staticDataProvider;
      _networkManager = networkManager;
      _gameScope = gameScope;
      _networkRoomModel = networkRoomModel;
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
      _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
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
      
      _networkRoomModel.SetConnectionState(obj.ConnectionState);
      _networkManager.ServerManager.Spawn((NetworkRoomModel)_networkRoomModel);

      _networkRoomModel.SetIsServer(true);
      _networkRoomModel.SetId(0);
      _networkScope.CreateChildFromPrefab(_gameScope);
    }

    private void OnClientConnectionState(ClientConnectionStateArgs obj)
    {
      _networkRoomModel.SetConnectionState(obj.ConnectionState);

      if (obj.ConnectionState == LocalConnectionState.Started)
      {
        _networkRoomModel.SetIsServer(false);
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