namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IPointMoveableEnemy : IWayPointEnemy, IMoveableEnemy
  {
    float WaitTime { get; }
  }
}