using UniRx;
using UnityEngine;
using VRShooter.Scopes;

namespace _Scripts.Gameplay.Enemies.Base
{
  public interface IAttackableEnemy : ITargetableEnemy{
    IEnemyAttacker Attacker { get; }
    IReadOnlyReactiveProperty<bool> IsAttacking { get; }
    float Damage { get; }
    float AttackRadius { get; }
    float AttackDelay { get; }
    int MaxComboCount { get; }
    Transform AttackPoint { get; }
    void SetAttacking(bool isAttacking);
  }
}