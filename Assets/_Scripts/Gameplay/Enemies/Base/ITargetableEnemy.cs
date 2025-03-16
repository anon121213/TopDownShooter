using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface ITargetableEnemy
  {
    Transform Target { get; set; }
  }
}