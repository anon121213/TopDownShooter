using System;
using _Scripts.Gameplay.Player;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkSyncService : NetworkBehaviour, INetworkSyncService, IInitializable, IDisposable
  {
    [Inject] private INetworkRoomModel _networkRoomModel;
    [Inject] private NetworkManager _networkManager;

    public void Initialize() => 
      _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;

    private void OnServerConnectionState(ServerConnectionStateArgs obj)
    {
      if (obj.ConnectionState != LocalConnectionState.Started)
        return;

      _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
      _networkManager.ServerManager.Spawn(this);
    }

    public void AddPlayer(int clientId)
    {
      _networkRoomModel.AddPlayer(clientId, new PlayerModel());
      RpcAddPlayer(clientId);
    }

    public void RemovePlayer(int clientId)
    {
      _networkRoomModel.RemovePlayer(clientId);
      RpcRemovePlayer(clientId);
    }

    public void AddClient(int clientId)
    {
      _networkRoomModel.AddClient(clientId);
      RpcAddClient(clientId);
    }

    public void RemoveClient(int clientId)
    {
      _networkRoomModel.RemoveClient(clientId);
      RpcRemoveClient(clientId);
    }

    [ObserversRpc]
    private void RpcAddClient(int clientId) => 
      _networkRoomModel.AddClient(clientId);

    [ObserversRpc]
    private void RpcRemoveClient(int clientId) => 
      _networkRoomModel.RemoveClient(clientId);
    
    [ObserversRpc]
    private void RpcAddPlayer(int clientId) => 
      _networkRoomModel.AddPlayer(clientId, new PlayerModel());

    [ObserversRpc]
    private void RpcRemovePlayer(int clientId) =>
      _networkRoomModel.RemovePlayer(clientId);
    
    public void Dispose() => 
      _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
  }

  public interface INetworkSyncService
  {
    void AddPlayer(int clientId);
    void RemovePlayer(int clientId);
    void AddClient(int clientId);
    void RemoveClient(int clientId);
  }
}