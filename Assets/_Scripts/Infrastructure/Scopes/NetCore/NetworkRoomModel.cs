using System.Collections.Generic;
using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Player;
using FishNet.Object;
using FishNet.Transporting;
using UniRx;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomModel : NetworkBehaviour, INetworkRoomModel
  {
    private readonly ReactiveProperty<int> _clientId = new();
    private readonly ReactiveProperty<LocalConnectionState> _connectionState = new();
    private readonly ReactiveProperty<bool> _isServer = new();
    private readonly ReactiveCollection<int> _clients = new();
    
    private readonly ReactiveDictionary<int, PlayerModelDTO> _playersDto = new();
    private readonly ReactiveDictionary<int, IPlayerModel> _playersRoot = new();
    private readonly ReactiveDictionary<int, IReadOnlyPlayerModel> _players = new();
    
    private readonly ReactiveDictionary<int, MobModelDataDTO> _mobsDto = new();
    private readonly ReactiveDictionary<int, IMobModel> _mobsRoot = new();
    private readonly ReactiveDictionary<int, IReadOnlyMobModel> _mobs = new();

    private readonly ReactiveProperty<bool> _isMobSpawnStarted = new();

    // READ-ONLY EXPOSE
    public IReadOnlyReactiveProperty<int> ClientId => _clientId;
    public IReadOnlyReactiveProperty<LocalConnectionState> ConnectionState => _connectionState;
    public new IReadOnlyReactiveProperty<bool> IsServer => _isServer;
    public IReadOnlyReactiveCollection<int> Clients => _clients;
    public IReadOnlyReactiveProperty<bool> IsMobSpawnStarted => _isMobSpawnStarted;

    public IReadOnlyReactiveDictionary<int, PlayerModelDTO> PlayersDto => _playersDto;
    public IReadOnlyReactiveDictionary<int, MobModelDataDTO> MobsDto => _mobsDto;
    public IReadOnlyReactiveDictionary<int, IReadOnlyPlayerModel> Players => _players;
    public IReadOnlyReactiveDictionary<int, IReadOnlyMobModel> Mobs => _mobs;

    public IReadOnlyReactiveDictionary<int, IPlayerModel> PlayersRoot => _playersRoot;
    public IReadOnlyReactiveDictionary<int, IMobModel> MobsRoot => _mobsRoot;


    // ---------------- SERVER WRITE API ----------------

    public void SetId(int id) => 
      _clientId.Value = id;

    public void SetIsServer(bool isServer) => 
      _isServer.Value = isServer;

    public void SetConnectionState(LocalConnectionState state)
    {
      if (!IsServerStarted) return;
      _connectionState.Value = state;
      RpcSetConnectionState(state);
    }

    public void AddClient(int clientId)
    {
      if (!IsServerStarted) return;

      if (_clients.Contains(clientId))
      {
        Debug.LogError($"[ROOM] DUPLICATE CLIENT ID: {clientId}");
        return;
      }

      _clients.Add(clientId);
      RpcAddClient(clientId);
    }

    public void RemoveClient(int clientId)
    {
      if (!IsServerStarted) return;

      if (!_clients.Contains(clientId))
      {
        Debug.LogError($"[ROOM] REMOVE CLIENT FAILED: ID {clientId} NOT FOUND");
        return;
      }

      _clients.Remove(clientId);
      RpcRemoveClient(clientId);
    }
    
    
    //-------------------PLAYERS------------------//


    public void AddDtoPlayer(PlayerModelDTO dto)
    {
      if (!IsServerStarted) return;

      if (!_playersDto.TryAdd(dto.ActorNumber, dto))
      {
        Debug.LogError($"[ROOM] DUPLICATE PLAYER DTO: Actor={dto.ActorNumber}");
        return;
      }

      RpcAddDtoPlayer(dto.ActorNumber, dto);
    }

    public void RemoveDtoPlayer(int actorNumber)
    {
      if (!IsServerStarted) return;

      if (!_playersDto.Remove(actorNumber))
        Debug.LogError($"[ROOM] REMOVE DTO FAILED: Actor={actorNumber}");

      RpcRemoveDtoPlayer(actorNumber);
    }

    // INVOKES ONLY LOCAL
    public void AddPlayer(IPlayerModel model)
    {
      if (!IsServerStarted) return;

      int id = model.ActorNumber.Value;

      if (!_players.TryAdd(id, model))
      {
        Debug.LogError($"[ROOM] DUPLICATE PLAYER MODEL: Actor={id}");
        return;
      }

      _playersRoot[id] = model;
    }

    public void RemovePlayer(int playerId)
    {
      if (!IsServerStarted) return;

      bool existed = false;

      existed |= _players.Remove(playerId);
      existed |= _playersRoot.Remove(playerId);
      existed |= _playersDto.Remove(playerId);

      if (!existed)
        Debug.LogError($"[ROOM] REMOVE PLAYER FAILED: Actor={playerId}");

      RpcRemovePlayer(playerId);
    }

    
    //-------------------MOBS-------------------//

    
    public void AddDtoMob(MobModelDataDTO data)
    {
      if (!IsServerStarted) return;

      if (!_mobsDto.TryAdd(data.ActorNumber, data))
      {
        Debug.LogError($"[ROOM] DUPLICATE MOB DTO: Actor={data.ActorNumber}");
        return;
      }

      RpcAddDtoMob(data.ActorNumber, data);
    }

    public void RemoveDtoMob(int actorNumber)
    {
      if (!IsServerStarted) return;

      if (!_mobsDto.Remove(actorNumber))
        Debug.LogError($"[ROOM] REMOVE DTO FAILED: Actor={actorNumber}");

      RpcRemoveDtoPlayer(actorNumber);
    }

    // INVOKES ONLY LOCAL
    public void AddMobLocal(IMobModel model)
    {
      if (!IsServerStarted) return;

      if (!_mobs.TryAdd(model.ActorNumber.Value, model))
      {
        Debug.LogError($"[ROOM] DUPLICATE MOB: Actor={model.ActorNumber.Value}");
        return;
      }

      _mobsRoot[model.ActorNumber.Value] = model;
    }

    public void RemoveMob(int actorNumber)
    {
      if (!IsServerStarted) return;

      bool existed = false;

      existed |= _mobs.Remove(actorNumber);
      existed |= _mobsRoot.Remove(actorNumber);

      if (!existed)
        Debug.LogError($"[ROOM] REMOVE MOB FAILED: Actor={actorNumber}");

      RpcRemoveMob(actorNumber);
    }

    public void SetIsMobSpawnStarted(bool v)
    {
      if (!IsServerStarted) return;
      _isMobSpawnStarted.Value = v;
      RpcSetIsMobSpawnStarted(v);
    }


    // ---------------- RPC SYNC ----------------

    [ObserversRpc] private void RpcSetConnectionState(LocalConnectionState v) => _connectionState.Value = v;

    [ObserversRpc] private void RpcAddClient(int id) => _clients.Add(id);
    [ObserversRpc] private void RpcRemoveClient(int id) => _clients.Remove(id);

    [ObserversRpc] private void RpcAddDtoPlayer(int id, PlayerModelDTO dto) => _playersDto[id] = dto;
    [ObserversRpc] private void RpcRemoveDtoPlayer(int id) => _playersDto.Remove(id);
    
    [ObserversRpc] private void RpcAddDtoMob(int id, MobModelDataDTO dto) => _mobsDto[id] = dto;
    [ObserversRpc] private void RpcRemoveDtoMob(int id) => _mobsDto.Remove(id);

    [ObserversRpc] private void RpcRemovePlayer(int id)
    {
      _players.Remove(id);
      _playersRoot.Remove(id);
      _playersDto.Remove(id);
    }

    [ObserversRpc] private void RpcRemoveMob(int id)
    {
      _mobs.Remove(id);
      _mobsRoot.Remove(id);
    }

    [ObserversRpc] private void RpcSetIsMobSpawnStarted(bool v) => _isMobSpawnStarted.Value = v;
  }
  
  public interface INetworkRoomModel : IReadOnlyNetworkRoomModel
  {
    void SetId(int id);
    void SetIsServer(bool isServer);
    void AddClient(int clientId);
    void RemoveClient(int clientId);
    void SetConnectionState(LocalConnectionState connectionState);

    void AddDtoPlayer(PlayerModelDTO data);
    void AddPlayer(IPlayerModel playerModel);
    void RemoveDtoPlayer(int actorNumber);
    void RemovePlayer(int playerId);
    
    void AddDtoMob(MobModelDataDTO data);
    void AddMobLocal(IMobModel model);
    void RemoveDtoMob(int actorNumber);
    void RemoveMob(int actorNumber);
    
    void SetIsMobSpawnStarted(bool isMobSpawnStarted);
  }
  
  public interface IReadOnlyNetworkRoomModel
  {
    IReadOnlyReactiveProperty<int> ClientId { get; }
    IReadOnlyReactiveProperty<LocalConnectionState> ConnectionState { get; }
    IReadOnlyReactiveProperty<bool> IsServer { get; }
    IReadOnlyReactiveCollection<int> Clients { get; }
    IReadOnlyReactiveProperty<bool> IsMobSpawnStarted { get; }

    IReadOnlyReactiveDictionary<int, PlayerModelDTO> PlayersDto { get; }
    IReadOnlyReactiveDictionary<int, MobModelDataDTO> MobsDto { get; }
    
    IReadOnlyReactiveDictionary<int, IReadOnlyPlayerModel> Players { get; }
    IReadOnlyReactiveDictionary<int, IReadOnlyMobModel> Mobs { get; }
    
    IReadOnlyReactiveDictionary<int, IPlayerModel> PlayersRoot { get; }
    IReadOnlyReactiveDictionary<int, IMobModel> MobsRoot { get; }
  }
}
