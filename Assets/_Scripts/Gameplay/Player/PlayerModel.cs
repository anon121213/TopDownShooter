using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Debuging;
using _Scripts.Infrastructure.Extensions;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class PlayerModel : NetworkBehaviour, IPlayerModel
  {
    private readonly ReactiveProperty<int> _actorNumber = new();
    private readonly ReactiveProperty<bool> _isLocal = new();
    private readonly ReactiveProperty<bool> _isSynced = new();
    private readonly ReactiveProperty<bool> _isDead = new();
    private readonly ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<int> ActorNumber => _actorNumber;
    public IReadOnlyReactiveProperty<bool> IsLocal => _isLocal;
    public IReadOnlyReactiveProperty<bool> IsSynced => _isSynced;
    public IReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public IReadOnlyReactiveProperty<float> Health => _health;

    public PlayerConfig PlayerConfig { get; private set; }

    private readonly SyncVar<PlayerModelDTO> _playerStateDto = new();
    
    public void Apply(PlayerModelDTO dto)
    {
      SetActorNumber(dto.ActorNumber);
      SetHealth(dto.Health);
      SetIsDead(dto.IsDead);
      _playerStateDto.Value = dto;
    }
    
    public override void OnStartClient()
    {
      PlayerModelRegistry.Models.Add(this);
      SetIsLocal(IsOwner);
      Apply(_playerStateDto.Value);
      _isSynced.Value = true;
    }
    
    // ---------------- LOCAL SETTERS ----------------

    public void SetIsLocal(bool isLocal) =>
      _isLocal.Value = isLocal;

    public void SetConfig(PlayerConfig playerConfig) =>
      PlayerConfig = playerConfig;
    
    // ---------------- SERVER API ----------------

    public void SetActorNumber(int actorNumber)
    {
      _actorNumber.Value = actorNumber;
      _playerStateDto.With(dto => dto.ActorNumber = actorNumber);
      RpcSetActorNumber(actorNumber);
    }

    public void SetIsDead(bool isDead)
    {
      _isDead.Value = isDead;
      _playerStateDto.With(dto => dto.IsDead = isDead);
      RpcSetIsDead(isDead);
    }

    public void SetHealth(float health)
    {
      _health.Value = Mathf.Clamp(health, 0, int.MaxValue);
      _playerStateDto.With(dto => dto.Health = _health.Value);
      RpcSetHealth(_health.Value);
    }
    
    // ---------------- CLIENT APPLIES STATE ----------------

    [ObserversRpc] private void RpcSetActorNumber(int actorNumber) => _actorNumber.Value = actorNumber;
    [ObserversRpc] private void RpcSetIsDead(bool isDead) => _isDead.Value = isDead;
    [ObserversRpc] private void RpcSetHealth(float health) => _health.Value = health;
    
    private void OnDestroy()
    {
      PlayerModelRegistry.Models.Remove(this);
    }
  }

  public struct PlayerModelDTO
  {
    public int ActorNumber;
    public bool IsDead;
    public float Health;

    public PlayerModelDTO(int actorNumber, bool isDead, float health)
    {
      ActorNumber = actorNumber;
      IsDead = isDead;
      Health = health;
    }

    public override string ToString() =>
      $"[MobDTO Actor={ActorNumber}, HP={Health}, IsDead={IsDead}]";
  }

  public interface IReadOnlyPlayerModel
  {
    PlayerConfig PlayerConfig { get; }
    IReadOnlyReactiveProperty<int> ActorNumber { get; }
    IReadOnlyReactiveProperty<bool> IsLocal { get; }
    IReadOnlyReactiveProperty<bool> IsDead { get; }
    IReadOnlyReactiveProperty<float> Health { get; }
    IReadOnlyReactiveProperty<bool> IsSynced { get; }
  }

  public interface IPlayerModel : IReadOnlyPlayerModel
  {
    void SetActorNumber(int actorNumber);
    void SetConfig(PlayerConfig playerConfig);
    void SetIsLocal(bool isLocal);
    void SetIsDead(bool isDead);
    void SetHealth(float health);
  }
}