using _Scripts.Gameplay.Enemies.Services;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IMoveableEnemy
  {
    IEnemyMover Mover { get; }
  }
}