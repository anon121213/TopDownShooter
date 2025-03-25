namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IPatrolEnemy : ITargetableEnemy
  {
    float CheckTargetRadius { get; } 
    float CheckTargetDelay { get; } 
  }
}