using System;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.health;
using _Scripts.Infrastructure.Services.Network;
using UniRx;
using UnityEngine;

namespace VRShooter.Scopes {
    public class EnemyAttacker : IEnemyAttacker {
        private readonly IAttackableEnemy _attackableEnemy;
        private readonly INetworkDamageService _networkDamageService;
        private readonly Collider[] _results = new Collider[128];
        private readonly ReactiveCommand<int> _onAttack = new();
        private readonly ReactiveProperty<int> _comboCount = new(1);
        private readonly SerialDisposable _serialDisposable = new();
        private readonly LayerMask _damageMask;

        public IReadOnlyReactiveProperty<int> ComboCount => _comboCount;
        public IObservable<int> OnAttack => _onAttack;
        
        public EnemyAttacker(IAttackableEnemy attackableEnemy, INetworkDamageService networkDamageService, LayerMask damageMask) {
            _attackableEnemy = attackableEnemy;
            _networkDamageService = networkDamageService;
            _damageMask = damageMask;
        }

        public bool TryAttack(Enemy enemy, Transform attackPoint) {
            Array.Clear(_results, 0, _results.Length);
            var count = Physics.OverlapSphereNonAlloc(attackPoint.position, _attackableEnemy.AttackRadius, _results, _damageMask);
            
            if (count <= 0)
                return false;
            
            for (var i = 0; i < count; i++) {
                if (!_results[i].TryGetComponent(out IDamageable damageable))
                    continue;

                _onAttack?.Execute(_comboCount.Value);
                SendDamage(damageable.ActorNumber.Value, _attackableEnemy.Damage);
                    
                if (_comboCount.Value >= _attackableEnemy.MaxComboCount)
                    _comboCount.Value = 1;
                else
                    _comboCount.Value++;

                return true;
            }

            return false;
        }

        private void SendDamage(int actorNumber, float damage) => 
            _networkDamageService.SendDamageToPlayer(new DamageData(actorNumber, damage));

        public void Dispose() => 
            _serialDisposable.Disposable?.Dispose();
    }

    public interface IEnemyAttacker {
        bool TryAttack(Enemy enemy, Transform attackPoint);
        IReadOnlyReactiveProperty<int> ComboCount { get; }
        IObservable<int> OnAttack { get; }
        void Dispose();
    }
}