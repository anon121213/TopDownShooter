using _Scripts.Gameplay.Enemies.Services;
using UnityEngine.AI;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IMoveableEnemy {
    IEnemyMover Mover { get; }
    NavMeshAgent NavMeshAgent { get; }
  }
}