using System.Collections.Generic;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Services;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies
{
  public class SimpleEnemy : Enemy, IMoveToPlayerEnemy, IPointMoveableEnemy, IAttackeableEnemy
  {
    public List<Transform> PatrolPoints { get; private set; } = new();
    public int CurrentPatrolIndex { get; set; }
    public Transform Target { get; set; }
    public float CheckTargetRadius { get; private set; }
    public float Speed { get; private set; }
    public float WaitTime { get; private set; }
    public float Damage { get; private set; }
    public float AttackRadius { get; private set; }
    public float AttackDelay { get; private set; }
    public float StartHealth { get; private set; }
    public IEnemyMover Mover { get; private set; }

    public void Construct(EnemyConfig config, IEnemyMover enemyMover)
    {
      CheckTargetRadius = config.CheckTargetRadius;
      Speed = config.Speed;
      WaitTime = config.WaitPatrolTime;
      Mover = enemyMover;
      Damage = config.Damage;
      AttackDelay = config.AttackDelay;
      AttackRadius = config.AttackRadius;
      StartHealth = config.StartHealth;
    }

    public void Initialize()
    {
      Health.OnHealthOver += Died;
      Health.Construct(StartHealth);
    }

    private void Died()
    {
      DisableEnemy();
      Health.OnHealthOver -= Died;
      Destroy(gameObject);
    }

    public void SetPatrolPoints(List<Transform> patrolPoints) => 
      PatrolPoints = patrolPoints;

    private void Update()
    {
      if (!Enabled)
        return;
      
      EnemyAI?.Execute();
    }

    private void OnDisable()
    {
      Health.OnHealthOver -= Died;
    }
  }
}