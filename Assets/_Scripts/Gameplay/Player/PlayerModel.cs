using _Scripts.Gameplay.Player.Data;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player
{
  public class PlayerModel : IPlayerModel
  {
    private readonly ReactiveProperty<int> _playerId = new();
    private readonly ReactiveProperty<bool> _isLocal = new();
    private readonly ReactiveProperty<bool> _isDead = new();
    private readonly ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<int> PlayerId => _playerId;
    public IReadOnlyReactiveProperty<bool> IsLocal => _isLocal;
    public IReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public IReadOnlyReactiveProperty<float> Health => _health;

    public PlayerConfig PlayerConfig { get; private set; }

    public void SetId(int id) =>
      _playerId.Value = id;
    
    public void SetConfig(PlayerConfig playerConfig) => 
      PlayerConfig = playerConfig;

    public void SetIsLocal(bool isLocal) =>
      _isLocal.Value = isLocal;

    public void SetIsDead(bool isDead) =>
      _isDead.Value = isDead;

    public void SetHealth(float health) =>
      _health.Value = Mathf.Clamp(health, 0, int.MaxValue);

    public void Apply(PlayerStateDTO dto)
    {
      SetIsLocal(dto.IsLocal);
      SetIsDead(dto.IsDead);
      SetHealth(dto.Health);
    }
  }

  public struct PlayerStateDTO
  {
    public readonly bool IsLocal;
    public readonly bool IsDead;
    public readonly float Health;

    public PlayerStateDTO(bool isLocal, bool isDead, float health)
    {
      IsLocal = isLocal;
      IsDead = isDead;
      Health = health;
    }
  }

  public interface IReadOnlyPlayerModel
  {
    PlayerConfig PlayerConfig { get; }
    IReadOnlyReactiveProperty<int> PlayerId { get; }
    IReadOnlyReactiveProperty<bool> IsLocal { get; }
    IReadOnlyReactiveProperty<bool> IsDead { get; }
    IReadOnlyReactiveProperty<float> Health { get; }
  }

  public interface IPlayerModel : IReadOnlyPlayerModel
  {
    void SetId(int id);
    void SetConfig(PlayerConfig playerConfig);
    void SetIsLocal(bool isLocal);
    void SetIsDead(bool isDead);
    void SetHealth(float health);
    void Apply(PlayerStateDTO dto);
  }
}