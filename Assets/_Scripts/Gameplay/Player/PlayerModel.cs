using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Debuging;
using FishNet.Object;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class PlayerModel : NetworkBehaviour, IPlayerModel
  {
    private readonly ReactiveProperty<int> _actorNumber = new();
    private readonly ReactiveProperty<bool> _isLocal = new();
    private readonly ReactiveProperty<bool> _isDead = new();
    private readonly ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<int> ActorNumber => _actorNumber;
    public IReadOnlyReactiveProperty<bool> IsLocal => _isLocal;
    public IReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public IReadOnlyReactiveProperty<float> Health => _health;

    public PlayerConfig PlayerConfig { get; private set; }

    public void Apply(PlayerStateDTO dto)
    {
      SetActorNumber(dto.ActorNumber);
      SetIsLocal(dto.IsLocal);
      SetHealth(dto.Health);
      SetIsDead(dto.IsDead);
    }
    
    // ---------------- Spawn callback ----------------
    
    public override void OnStartClient()
    {
      base.OnStartClient();
      SetIsLocal(IsOwner);
      _ = PlayerModelRegistry.Models.Add(this);
    }
    
    // ---------------- LOCAL SETTERS ----------------

    public void SetIsLocal(bool isLocal) =>
      _isLocal.Value = isLocal;

    public void SetConfig(PlayerConfig playerConfig) =>
      PlayerConfig = playerConfig;
    
    // ---------------- SERVER API ----------------

    public void SetActorNumber(int actorNumber)
    {
      if (!IsHostStarted) return;
      _actorNumber.Value = actorNumber;
      RpcSetActorNumber(actorNumber);
    }

    public void SetIsDead(bool isDead)
    {
      if (!IsServerStarted) return;
      _isDead.Value = isDead;
      RpcSetIsDead(isDead);
    }

    public void SetHealth(float health)
    {
      if (!IsServerStarted) return;
      _health.Value = Mathf.Clamp(health, 0, int.MaxValue);
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

  public readonly struct PlayerStateDTO
  {
    public readonly int ActorNumber;
    public readonly bool IsLocal;
    public readonly bool IsDead;
    public readonly float Health;

    public PlayerStateDTO(int actorNumber, bool isLocal, bool isDead, float health)
    {
      ActorNumber = actorNumber;
      IsLocal = isLocal;
      IsDead = isDead;
      Health = health;
    }
    
    public override string ToString() =>
      $"[MobDTO Actor={ActorNumber}, IsLocal={IsLocal}, HP={Health}, IsDead={IsDead}]";
  }

  public interface IReadOnlyPlayerModel
  {
    PlayerConfig PlayerConfig { get; }
    IReadOnlyReactiveProperty<int> ActorNumber { get; }
    IReadOnlyReactiveProperty<bool> IsLocal { get; }
    IReadOnlyReactiveProperty<bool> IsDead { get; }
    IReadOnlyReactiveProperty<float> Health { get; }
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