using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Services;
using UniRx;
using UnityEngine;
using VRShooter.Scopes;

namespace _Scripts.Gameplay.Enemies
{
  public class SimpleEnemy : Enemy, IChasingEnemy, IAttackableEnemy, IPlayerNetworkTargetableEnemy
  {
    [field: SerializeField] public Transform AttackPoint { get; private set; }

    private readonly ReactiveDictionary<int, TargetData> _targets = new();
    private readonly ReactiveProperty<TargetData> _currentTarget = new();
    private readonly ReactiveProperty<bool> _isChasing = new();
    private readonly ReactiveProperty<bool> _isAttacking = new();

    public float Damage { get; private set; }
    public float AttackRadius { get; private set; }
    public float AttackDelay { get; private set; }
    public float StartHealth { get; private set; }
    public float StoppingDistance { get; private set; }
    public int MaxComboCount { get; private set; }

    public IEnemyMover Mover { get; private set; }
    public IEnemyAttacker Attacker { private set; get; }
    public IEnemyTargetSetter TargetSetter { private set; get; }

    public IReadOnlyReactiveProperty<bool> IsChasing => _isChasing;
    public IReadOnlyReactiveProperty<bool> IsAttacking => _isAttacking;
    public IReadOnlyReactiveDictionary<int, TargetData> Targets => _targets;
    public IReadOnlyReactiveProperty<TargetData> CurrentTarget => _currentTarget;

    public void Construct(EnemyData config, IEnemyMover enemyMover, IEnemyAttacker enemyAttacker,
      IEnemyTargetSetter enemyTargetSetter)
    {
      Mover = enemyMover;
      Attacker = enemyAttacker;
      TargetSetter = enemyTargetSetter;
      Damage = config.Damage;
      AttackDelay = config.AttackDelay;
      AttackRadius = config.AttackRadius;
      StartHealth = config.StartHealth;
      StoppingDistance = config.StoppingDistance;
      MaxComboCount = config.MaxComboCount;
    }

    protected override void OnSetContext(Context context)
    {
      base.OnSetContext(context);
      MobModel.IsDead.Subscribe(isDead =>
      {
        if (isDead)
        {
          DisableEnemy();
        }
      }).AddTo(ViewDisposables);
    }

    public void TryAddTarget(TargetData targetData) => 
      _targets.Add(targetData.ActorNumber, targetData);

    public void TryRemoveTarget(int actorNumber)
    {
      if (!_targets.Remove(actorNumber))
        return;
      
      if (actorNumber == _currentTarget.Value.ActorNumber) 
        SetCurrentTarget(-1, null);
    }

    public void ResetTargets() =>
      _targets?.Clear();

    public void SetCurrentTarget(int actorNumber, Transform target)
    {
      if (actorNumber == -1)
      {
        _currentTarget.Value = new TargetData(-1, null);
        return;
      } 
      
      _currentTarget.Value = _targets[actorNumber];

      if (target)
        return;

      _isChasing.Value = false;
      _isAttacking.Value = false;
    }

    public void SetChasing(bool isChasing)
    {
      _isChasing.Value = isChasing;

      if (isChasing)
        _isAttacking.Value = false;
    }

    public void SetAttacking(bool isAttacking)
    {
      _isAttacking.Value = isAttacking;

      if (isAttacking)
        _isChasing.Value = false;
    }

    public override void OnGetFromPool()
    {
      base.OnGetFromPool();
      _targets.Clear();
      SetCurrentTarget(-1, null);
      _isChasing.Value = false;
    }

    public override void OnReturnToPool()
    {
      base.OnReturnToPool();
      _targets.Clear();
    }

    public override void DisableEnemy()
    {
      base.DisableEnemy();
      _isChasing.Value = false;
      _isAttacking.Value = false;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      Attacker?.Dispose();
    }
  }
}