using System;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Infrastructure.Services.Network;
using UniRx;
using UnityEngine;

namespace VRShooter.Scopes {
    public class EnemyAttacker : IEnemyAttacker {
        private readonly IAttackableEnemy _attackableEnemy;
        private readonly INetworkDamageService _networkDamageService;
        private readonly ReactiveCommand<int> _onAttack = new();
        private readonly SerialDisposable _serialDisposable = new();

        public IObservable<int> OnAttack => _onAttack;
        
        public EnemyAttacker(IAttackableEnemy attackableEnemy, INetworkDamageService networkDamageService) {
            _attackableEnemy = attackableEnemy;
            _networkDamageService = networkDamageService;
        }

        public bool TryAttack(int attackActorNumber, Transform attackEntity) {
            if (Vector3.Distance(_attackableEnemy.AttackPoint.position, attackEntity.position) > _attackableEnemy.AttackRadius)
                return false;

            SendDamage(attackActorNumber, _attackableEnemy.Damage);
            return true;
        }

        private void SendDamage(int actorNumber, float damage) => 
            _networkDamageService.SendDamageToPlayer(new DamageData(actorNumber, damage));

        public void Dispose() => 
            _serialDisposable.Disposable?.Dispose();
    }

    public interface IEnemyAttacker {
        bool TryAttack(int attackActorNumber, Transform attackEntity);
        IObservable<int> OnAttack { get; }
        void Dispose();
    }
}