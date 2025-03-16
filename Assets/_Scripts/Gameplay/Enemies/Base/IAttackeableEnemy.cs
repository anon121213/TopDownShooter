namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IAttackeableEnemy
  {
    float Damage { get; }
    float AttackRadius { get; }
    float AttackDelay { get; }
  }
}