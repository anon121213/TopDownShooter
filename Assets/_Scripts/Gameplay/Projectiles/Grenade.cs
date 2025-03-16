using System;
using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Projectiles.Data;
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
    private readonly Collider[] _results = new Collider[30];

    public override event Action<Projectile> OnCollide;

    public override void Construct(ProjectileConfig projectileConfig)
    {
      GrenadeProjectileConfig config = (GrenadeProjectileConfig)projectileConfig;

      _damage = projectileConfig.Damage;
      _speed = projectileConfig.Force;
      _radius = config.Radius;
      _delay = config.Delay;
    }

    public override void Initialize()
    {
      _disposable?.Dispose();
      Vector3 throwDirection = transform.forward;
      _rb.AddForce(throwDirection * _speed, ForceMode.Impulse);
      _disposable = Observable.Timer(TimeSpan.FromSeconds(_delay))
        .Subscribe(_ => TakeDamage()).AddTo(this);
    }

    private void TakeDamage()
    {
      int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, _results);

      for (int i = 0; i < count; i++)
        if (_results[i].TryGetComponent(out IDamageable damageable))
          damageable.TakeDamage(_damage);

      OnCollide?.Invoke(this);
    }
  }
}