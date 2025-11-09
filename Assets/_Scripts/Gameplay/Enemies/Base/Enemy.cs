using _Scripts.Gameplay.Enemies.BehaviourTree;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.health;
using FishNet.Object;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Base
{
  public abstract class Enemy : NetworkBehaviour, IDamageable
  {
    [field: SerializeField] public MobModel MobModel { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    protected EnemyAI EnemyAI { get; private set; }
    public EnemyData Config { get; private set; }

    private readonly ReactiveProperty<bool> _isEnabled = new();
    private readonly ReactiveProperty<bool> _isPooled = new();

    public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;
    public IReadOnlyReactiveProperty<bool> IsPooled => _isPooled;

    protected readonly CompositeDisposable ViewDisposables = new();

    public IReadOnlyReactiveProperty<int> ActorNumber => MobModel.ActorNumber;
    public IReadOnlyReactiveProperty<bool> IsDead => MobModel.IsDead;

    public void SetContext(Context context)
    {
      Config = context.Config;
      OnSetContext(context);
    }

    public void SetAI(EnemyAI ai)
    {
      if (EnemyAI != null)
        return;

      EnemyAI = ai;
    }

    protected virtual void Update()
    {
      if (!IsEnabled.Value)
        return;

      EnemyAI?.Execute();
    }

    protected virtual void OnSetContext(Context context) { }


    public virtual void OnGetFromPool()
    {
      ViewDisposables.Clear();
      _isPooled.Value = false;
    }

    public virtual void OnReturnToPool()
    {
      ViewDisposables.Clear();
      _isPooled.Value = true;
    }

    public virtual void EnableEnemy()
    {
      EnemyAI?.SetEnable(true);
      _isEnabled.Value = true;
      NavMeshAgent.enabled = true;
    }

    public virtual void DisableEnemy()
    {
      EnemyAI?.SetEnable(false);
      _isEnabled.Value = false;
      NavMeshAgent.enabled = false;
    }

    protected virtual void OnDestroy()
    {
      EnemyAI?.Dispose();
      ViewDisposables.Dispose();
    }
  }

  public struct Context
  {
    public readonly EnemyData Config;

    public Context(EnemyData enemyData)
    {
      Config = enemyData;
    }
  }
}