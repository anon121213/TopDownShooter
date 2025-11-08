using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Services
{
  public class EnemyMover : IEnemyMover
  {
    private const float MinUpdateDistance = 0.1f;
    private readonly NavMeshAgent _agent;
    private readonly ReactiveProperty<bool> _isMoving = new();

    private Vector3 _lastTarget = Vector3.positiveInfinity;

    public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
        
    public EnemyMover(IMoveableEnemy moveableEnemy, EnemyData config)
    {
      _agent = moveableEnemy.NavMeshAgent;
      _agent.speed = config.Speed;
      _agent.acceleration = config.Acceleration;
      _agent.angularSpeed = config.AngularSpeed;
      _agent.stoppingDistance = config.StoppingDistance;
    }

    public void Move(Vector3 target) {
      if (!_agent || !_agent.enabled) return;
      _isMoving.Value = true;
            
      if (!(Vector3.Distance(_lastTarget, target) > MinUpdateDistance)) return;
      _lastTarget = target;
      _agent.isStopped = false;

      if (NavMesh.SamplePosition(target, out var hit, 10f, NavMesh.AllAreas))
        _agent.SetDestination(hit.position);
    }

    public void Stop()
    {
      _isMoving.Value = false;
      _lastTarget = Vector3.positiveInfinity;
           
      if (!_agent || !_agent.enabled || !_agent.isOnNavMesh || _agent.isStopped)
        return;

      _agent.isStopped = true;
      _agent.velocity = Vector3.zero; 
    }
  }
    
  public interface IEnemyMover
  {
    IReadOnlyReactiveProperty<bool> IsMoving { get; }
    void Move(Vector3 target);
    void Stop();
  }
}