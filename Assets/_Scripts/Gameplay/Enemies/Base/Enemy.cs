using _Scripts.Gameplay.Enemies.BehaviourTree;
using _Scripts.Gameplay.Enemies.Data;
using FishNet.Object;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Base
{
  public abstract class Enemy : NetworkBehaviour
  {
    [field: SerializeField] public MobModel MobModel { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    protected EnemyAI EnemyAI { get; private set; }
    public EnemyData Config { get; private set; }

    private readonly ReactiveProperty<bool> _isEnabled = new ReactiveProperty<bool>();
    private readonly ReactiveProperty<bool> _isPooled = new ReactiveProperty<bool>();

    public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;
    public IReadOnlyReactiveProperty<bool> IsPooled => _isPooled;

    public readonly CompositeDisposable ViewDisposables = new CompositeDisposable();
    private readonly SerialDisposable _avatarSerialDisposable = new SerialDisposable();

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
      _avatarSerialDisposable.Dispose();
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