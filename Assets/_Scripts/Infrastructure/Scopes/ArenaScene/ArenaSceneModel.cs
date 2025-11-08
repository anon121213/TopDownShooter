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
    private readonly ReactiveDictionary<int, PlayerRootView> _players = new();
    private readonly ReactiveDictionary<int, Enemy> _enemies = new();

    private readonly ReactiveDictionary<int, PlayerScope> _playersScopes = new();
    private readonly ReactiveDictionary<int, MobScope> _mobScopes = new();
    
    public IReadOnlyReactiveDictionary<int, PlayerRootView> Players => _players;
    public IReadOnlyReactiveDictionary<int, Enemy> Enemies => _enemies;
    
    public IReadOnlyReactiveDictionary<int, PlayerScope> PlayersScopes => _playersScopes;
    public IReadOnlyReactiveDictionary<int, MobScope> MobScopes => _mobScopes;
    
    public void AddPlayerScope(int actorNumber, PlayerScope playerView)
    {
      if (_playersScopes.TryAdd(actorNumber, playerView)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} already exists!");
    }
    
    public void RemovePlayerScope(int actorNumber)
    {
      if (_playersScopes.Remove(actorNumber)) 
        return;
      
      Debug.LogError($"PlayerScope with actor number {actorNumber} does not exists!");
    }
    
    public void AddMobScope(int actorNumber, MobScope playerView)
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

    public void AddPlayer(PlayerRootView playerRootView)
    {
      if (_players.TryAdd(playerRootView.ActorNumber.Value, playerRootView)) 
        return;
      
      Debug.LogError($"Player with actor number {playerRootView.ActorNumber.Value} already exists!");
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
    public IReadOnlyReactiveDictionary<int, PlayerScope> PlayersScopes { get; }
    public IReadOnlyReactiveDictionary<int, MobScope> MobScopes { get; }
    
    void AddPlayerScope(int actorNumber, PlayerScope playerView);
    void RemovePlayerScope(int actorNumber);
    
    void AddMobScope(int actorNumber, MobScope playerView);
    void RemoveMobScope(int actorNumber);
    
    void AddPlayer(PlayerRootView playerRootView);
    void RemovePlayer(int actorNumber);
    
    void AddMob(Enemy playerView);
    void RemoveMob(int actorNumber);
  }

  public interface IRaedOnlyArenaSceneModel
  {
    IReadOnlyReactiveDictionary<int, PlayerRootView> Players { get; }
    IReadOnlyReactiveDictionary<int, Enemy> Enemies { get; }
  }
}