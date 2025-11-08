using System.Collections.Generic;
using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Player;
using UniRx;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.ArenaScene
{
  public class ArenaSceneModel : IArenaSceneModel
  {
    private readonly ReactiveDictionary<int, NetworkPlayerView> _players = new();
    private readonly ReactiveDictionary<int, Enemy> _enemies = new();

    private readonly ReactiveDictionary<int, LocalPlayerScope> _localPlayersScopes = new();
    private readonly ReactiveDictionary<int, RemotePlayerScope> _remotePlayerScopes = new();
    private readonly ReactiveDictionary<int, LocalMobScope> _mobScopes = new();
    
    public IReadOnlyReactiveDictionary<int, NetworkPlayerView> Players => _players;
    public IReadOnlyReactiveDictionary<int, Enemy> Enemies => _enemies;
    
    public IReadOnlyReactiveDictionary<int, LocalPlayerScope> LocalPlayersScopes => _localPlayersScopes;
    public IReadOnlyReactiveDictionary<int, RemotePlayerScope> RemotePlayersScopes => _remotePlayerScopes;
    public IReadOnlyReactiveDictionary<int, LocalMobScope> MobScopes => _mobScopes;
    
    public void AddLocalPlayerScope(int actorNumber, LocalPlayerScope localPlayerScope)
    {
      if (_localPlayersScopes.TryAdd(actorNumber, localPlayerScope)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} already exists!");
    }

    public void AddRemotePlayerScope(int actorNumber, RemotePlayerScope remotePlayerScope)
    {
      if (_remotePlayerScopes.TryAdd(actorNumber, remotePlayerScope)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} already exists!");
    }

    public void RemoveLocalPlayerScope(int actorNumber)
    {
      if (_localPlayersScopes.Remove(actorNumber)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} does not exists!");
    }
    
    public void RemoveRemotePlayerScope(int actorNumber)
    {
      if (_remotePlayerScopes.Remove(actorNumber)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} does not exists!");
    }

    public void AddMobScope(int actorNumber, LocalMobScope playerView)
    {
      if (_mobScopes.TryAdd(actorNumber, playerView)) 
        return;
      
      Debug.LogError($"EnemyScope with actor number {actorNumber} already exists!");
    }
    
    public void RemoveMobScope(int actorNumber)
    {
      if (_mobScopes.Remove(actorNumber)) 
        return;
      
      Debug.LogError($"EnemyScope with actor number {actorNumber} does not exists!");
    }

    public void AddPlayer(NetworkPlayerView networkPlayerView)
    {
      if (_players.TryAdd(networkPlayerView.ActorNumber.Value, networkPlayerView)) 
        return;
      
      Debug.LogError($"Player with actor number {networkPlayerView.ActorNumber.Value} already exists!");
    }
    
    public void RemovePlayer(int actorNumber)
    {
      if (_players.Remove(actorNumber))
        return; 
      
      Debug.LogError($"Player with actor number {actorNumber} does not exists!");
    }
    
    public void AddMob(Enemy playerView)
    {
      if (_enemies.TryAdd(playerView.MobModel.ActorNumber.Value, playerView)) 
        return;
      
      Debug.LogError($"Enemy with actor number {playerView.MobModel.ActorNumber.Value} already exists!");
    }
    
    public void RemoveMob(int actorNumber)
    {
      if (_enemies.Remove(actorNumber)) 
        return;
      
      Debug.LogError($"Enemy with actor number {actorNumber} does not exists!");
    }
  }
  
  public interface IArenaSceneModel : IRaedOnlyArenaSceneModel
  {
    public IReadOnlyReactiveDictionary<int, LocalMobScope> MobScopes { get; }
    public IReadOnlyReactiveDictionary<int, LocalPlayerScope> LocalPlayersScopes { get; }
    public IReadOnlyReactiveDictionary<int, RemotePlayerScope> RemotePlayersScopes { get; }

    void AddLocalPlayerScope(int actorNumber, LocalPlayerScope localPlayerScope);
    void AddRemotePlayerScope(int actorNumber, RemotePlayerScope remotePlayerScope);
    void RemoveLocalPlayerScope(int actorNumber);
    void RemoveRemotePlayerScope(int actorNumber);
    
    void AddMobScope(int actorNumber, LocalMobScope playerView);
    void RemoveMobScope(int actorNumber);
    
    void AddPlayer(NetworkPlayerView networkPlayerView);
    void RemovePlayer(int actorNumber);
    
    void AddMob(Enemy playerView);
    void RemoveMob(int actorNumber);
  }

  public interface IRaedOnlyArenaSceneModel
  {
    IReadOnlyReactiveDictionary<int, NetworkPlayerView> Players { get; }
    IReadOnlyReactiveDictionary<int, Enemy> Enemies { get; }
  }
}