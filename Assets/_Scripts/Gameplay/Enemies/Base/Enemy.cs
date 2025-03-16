using System;
using _Scripts.Gameplay.Enemies.BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Base
{
  public abstract class Enemy : MonoBehaviour, IDisposable
  {
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    protected EnemyAI EnemyAI { get; private set; }
    public bool Enabled { get; private set; }

    public void SetAI(EnemyAI ai)
    {
      if (EnemyAI != null)
        return;
      
      EnemyAI = ai;
    }

    public virtual void EnableEnemy()
    {
      EnemyAI?.SetEnable(true);
      Enabled = true;
    }

    public virtual void DisableEnemy()
    {
      EnemyAI?.SetEnable(false);
      Enabled = false;
    }

    public void Dispose()
    {
      EnemyAI?.Dispose();
    }
  }
}