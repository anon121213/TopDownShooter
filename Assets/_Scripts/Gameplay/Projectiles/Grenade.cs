using System;
using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Infrastructure.Services.Network;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles
{
  public class Grenade : Projectile
  {
    [SerializeField] private Rigidbody _rb;

    private float _damage;
    private float _speed;
    private float _radius;
    private float _delay;
    private IDisposable _disposable;
    private INetworkDamageService _damageService;
    private readonly Collider[] _results = new Collider[128];

    public override event Action<Projectile> OnReturnToPool;

    public override void Construct(ProjectileData projectileData, INetworkDamageService damageService)
    {
      _damage = projectileData.Damage;
      _speed = projectileData.Speed;
      _radius = projectileData.ExplosionRadius;
      _delay = projectileData.ExplosionDelay;
      _damageService = damageService;
    }

    public override void Initialize()
    {
      if (!IsServerStarted)
        return;
      
      _disposable?.Dispose();
      var throwDirection = transform.forward;
      _rb.AddForce(throwDirection * _speed, ForceMode.Impulse);
      _disposable = Observable.Timer(TimeSpan.FromSeconds(_delay))
        .Subscribe(_ => TakeDamage()).AddTo(this);
    }

    private void TakeDamage()
    {
      if (!IsServerStarted)
        return;
        
      int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _results);

      for (int i = 0; i < count; i++)
        if (_results[i].TryGetComponent(out IDamageable damageable))
          _damageService.SendDamageToEnemy(new DamageData(damageable.ActorNumber.Value, _damage));

      OnReturnToPool?.Invoke(this);
    }
  }
}