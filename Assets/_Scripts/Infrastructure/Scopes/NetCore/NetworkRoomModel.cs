using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Player;
using FishNet.Transporting;
using UniRx;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomModel : INetworkRoomModel
  {
    private readonly ReactiveProperty<int> _clientId = new();
    private readonly ReactiveProperty<LocalConnectionState> _connectionState = new();
    private readonly ReactiveProperty<bool> _isServer = new();
    private readonly ReactiveDictionary<int, IReadOnlyPlayerModel> _players = new();
    private readonly ReactiveDictionary<int, IPlayerModel> _playersRoot = new();
    private readonly ReactiveCollection<int> _clients = new();

    public IReadOnlyReactiveProperty<int> ClientId => _clientId;
    public IReadOnlyReactiveProperty<LocalConnectionState> ConnectionState => _connectionState;
    public IReadOnlyReactiveProperty<bool> IsServer => _isServer;
    public IReadOnlyReactiveDictionary<int, IReadOnlyPlayerModel> Players => _players;
    public IReadOnlyReactiveDictionary<int, IPlayerModel> PlayersRoot => _playersRoot;
    public IReadOnlyReactiveCollection<int> Clients => _clients;

    public void SetId(int id) =>
      _clientId.Value = id;

    public void AddPlayer(int playerId, IPlayerModel playerEntryPoint)
    {
      if (_players.TryAdd(playerId, playerEntryPoint))
        return;
      
      Debug.LogError($"Player with Id {playerId} already exists!");
    }

    public void RemovePlayer(int playerId)
    {
      if (!_players.Remove(playerId))
        return;
      
      Debug.LogError($"Player with Id {playerId} does not exist!");
    }

    public void AddClient(int clientId)
    {
      if (_clients.Contains(clientId))
      {
        Debug.LogError($"Client with Id {clientId} already exists!");
        return;
      }
      
      _clients.Add(clientId);
    }

    public void RemoveClient(int clientId)
    {
      if (!_clients.Contains(clientId))
      {
        Debug.LogError($"Client with Id {clientId} does not exist!");
        return;
      }
      
      _clients.Remove(clientId);
    }

    public void SetIsServer(bool isServer) =>
      _isServer.Value = isServer;

    public void SetConnectionState(LocalConnectionState connectionState) =>
      _connectionState.Value = connectionState;
    
    public string InstanceTag { get; } = Guid.NewGuid().ToString("N");
  }

  public interface INetworkRoomModel : IReadOnlyNetworkRoomModel
  {
    IReadOnlyReactiveDictionary<int, IPlayerModel> PlayersRoot { get; }

    void SetId(int id);
    void AddPlayer(int playerId, IPlayerModel playerEntryPoint);
    void RemovePlayer(int playerId);
    void AddClient(int clientId);
    void RemoveClient(int clientId);
    void SetIsServer(bool isServer);
    void SetConnectionState(LocalConnectionState connectionState);
  }
  
  public interface IReadOnlyNetworkRoomModel
  {
    string InstanceTag { get; }
    IReadOnlyReactiveProperty<int> ClientId { get; }
    IReadOnlyReactiveProperty<LocalConnectionState> ConnectionState { get; }
    IReadOnlyReactiveProperty<bool> IsServer { get; }
    IReadOnlyReactiveDictionary<int, IReadOnlyPlayerModel> Players { get; }
    IReadOnlyReactiveCollection<int> Clients { get; }
  }
}