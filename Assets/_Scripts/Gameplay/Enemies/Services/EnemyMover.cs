using _Scripts.Gameplay.Enemies.Data;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Services
{
  public class EnemyMover : IEnemyMover
  {
    private readonly NavMeshAgent _agent;
    private readonly float _speed;

    public EnemyMover(NavMeshAgent agent, EnemyConfig enemyConfig)
    {
      _agent = agent;
      _speed = enemyConfig.Speed;
    }
    
    public void Move(Vector3 target)
    {
      _agent.speed = _speed;
      _agent.SetDestination(target);
    }
  }

  public interface IEnemyMover
  {
    void Move(Vector3 target);
  }
}