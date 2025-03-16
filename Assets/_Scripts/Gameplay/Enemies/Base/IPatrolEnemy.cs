namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IPatrolEnemy : ITargetableEnemy
  {
    float CheckTargetRadius { get; } 
  }
}