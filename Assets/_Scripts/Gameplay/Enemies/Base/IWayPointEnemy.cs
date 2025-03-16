using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IWayPointEnemy
  {
    List<Transform> PatrolPoints { get; }
    int CurrentPatrolIndex { get; set; }
  }
}