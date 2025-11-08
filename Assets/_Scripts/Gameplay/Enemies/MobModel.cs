using _Scripts.Infrastructure.Debuging;
using _Scripts.Infrastructure.Extensions;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies
{
  public class MobModel : NetworkBehaviour, IMobModel
  {
    private readonly ReactiveProperty<int> _mobActorNumber = new();
    private readonly ReactiveProperty<MobBehaviourTypes> _behaviourType = new();
    private readonly ReactiveProperty<MobType> _mobType = new();
    private readonly ReactiveProperty<Vector3> _spawnPosition = new();
    private readonly ReactiveProperty<float> _health = new();
    private readonly ReactiveProperty<bool> _isDead = new();
    private readonly ReactiveProperty<bool> _isEnable = new(true);

    public IReadOnlyReactiveProperty<int> ActorNumber => _mobActorNumber;
    public IReadOnlyReactiveProperty<MobBehaviourTypes> BehaviourType => _behaviourType;
    public IReadOnlyReactiveProperty<MobType> MobType => _mobType;
    public IReadOnlyReactiveProperty<Vector3> SpawnPosition => _spawnPosition;
    public IReadOnlyReactiveProperty<float> Health => _health;
    public IReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public IReadOnlyReactiveProperty<bool> IsEnable => _isEnable;

    private readonly SyncVar<MobModelDataDTO> _mobStateDto = new();
    
    public void Apply(MobModelDataDTO dto)
    {
      SetActorNumber(dto.ActorNumber);
      SetMobType(dto.MobType);
      SetBehType(dto.BehaviourType);
      SetSpawnPosition(dto.SpawnPosition);
      SetHealth(dto.Health);
      SetIsDead(dto.Health <= 0);
      SetIsEnable(dto.IsEnable);
      _mobStateDto.Value = dto;
    }
    
    // ---------------- Spawn callback ----------------
    
    public override void OnStartClient()
    {
      MobModelRegistry.Models.Add(this);
      Apply(_mobStateDto.Value);
    }

    // ---------------- SERVER API ----------------

    public void SetActorNumber(int actorNumber)
    {
      if (!IsServerStarted) return;

      _mobActorNumber.Value = actorNumber;
      _mobStateDto.With(dto => dto.ActorNumber = actorNumber);
      RpcSetActorNumber(actorNumber);
    }

    public void SetMobType(MobType mobType)
    {
      if (!IsServerStarted) return;

      _mobType.Value = mobType;
      _mobStateDto.With(dto => dto.MobType = mobType);
      RpcSetMobType(mobType);
    }

    public void SetBehType(MobBehaviourTypes behaviourType)
    {
      if (!IsServerStarted) return;

      _behaviourType.Value = behaviourType;
      _mobStateDto.With(dto => dto.BehaviourType = behaviourType);
      RpcSetBehType(behaviourType);
    }

    public void SetSpawnPosition(Vector3 spawnPosition)
    {
      if (!IsServerStarted) return;

      _spawnPosition.Value = spawnPosition;
      _mobStateDto.With(dto => dto.SpawnPosition = spawnPosition);
      RpcSetSpawnPosition(spawnPosition);
    }

    public void SetHealth(float health)
    {
      if (!IsServerStarted) return;

      health = Mathf.Clamp(health, 0, int.MaxValue);
      _health.Value = health;
      _mobStateDto.With(dto => dto.Health = health);
      RpcSetHealth(health);

      if (health <= 0) 
        SetIsDead(true);
    }

    public void SetIsDead(bool isDead)
    {
      if (!IsServerStarted) return;

      _isDead.Value = isDead;
      RpcSetIsDead(isDead);
    }

    public void SetIsEnable(bool isEnable)
    {
      if (!IsServerStarted) return;

      _isEnable.Value = isEnable;
      _mobStateDto.With(dto => dto.IsEnable = isEnable);
      RpcSetIsEnable(isEnable);
    }

    // ---------------- CLIENT RPC ----------------

    [ObserversRpc] private void RpcSetActorNumber(int actorNumber) => _mobActorNumber.Value = actorNumber;
    [ObserversRpc] private void RpcSetMobType(MobType mobType) => _mobType.Value = mobType;
    [ObserversRpc] private void RpcSetBehType(MobBehaviourTypes behaviourTypes) => _behaviourType.Value = behaviourTypes;
    [ObserversRpc] private void RpcSetSpawnPosition(Vector3 spawnPosition) => _spawnPosition.Value = spawnPosition;
    [ObserversRpc] private void RpcSetHealth(float health) => _health.Value = health;
    [ObserversRpc] private void RpcSetIsDead(bool isDead) => _isDead.Value = isDead;
    [ObserversRpc] private void RpcSetIsEnable(bool isEnable) => _isEnable.Value = isEnable;
    
    private void OnDestroy()
    {
      MobModelRegistry.Models.Remove(this);
    }
  }


  public struct MobModelDataDTO
  {
    public int ActorNumber;
    public MobType MobType;
    public MobBehaviourTypes BehaviourType;
    public Vector3 SpawnPosition;
    public float Health;
    public bool IsEnable;

    public MobModelDataDTO(int actorNumber, MobType mobType, MobBehaviourTypes behaviourType, Vector3 spawnPosition, float health, bool isEnable = false)
    {
      ActorNumber = actorNumber;
      MobType = mobType;
      BehaviourType = behaviourType;
      SpawnPosition = spawnPosition;
      Health = health;
      IsEnable = isEnable;
    }

    public override string ToString() =>
      $"[MobDTO Actor={ActorNumber}, Type={MobType}, Beh={BehaviourType}, HP={Health}, Pos={SpawnPosition}]";
  }


  public interface IReadOnlyMobModel
  {
    IReadOnlyReactiveProperty<int> ActorNumber { get; }
    IReadOnlyReactiveProperty<MobBehaviourTypes> BehaviourType { get; }
    IReadOnlyReactiveProperty<MobType> MobType { get; }
    IReadOnlyReactiveProperty<Vector3> SpawnPosition { get; }
    IReadOnlyReactiveProperty<float> Health { get; }
    IReadOnlyReactiveProperty<bool> IsDead { get; }
    IReadOnlyReactiveProperty<bool> IsEnable { get; }
  }

  public interface IMobModel : IReadOnlyMobModel
  {
    void Apply(MobModelDataDTO dto);
    void SetActorNumber(int actorNumber);
    void SetMobType(MobType mobType);
    void SetBehType(MobBehaviourTypes behaviourTypes);
    void SetSpawnPosition(Vector3 spawnPosition);
    void SetHealth(float health);
    void SetIsDead(bool isDead);
    void SetIsEnable(bool isEnable);
  }
}
