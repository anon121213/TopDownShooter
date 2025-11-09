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

    private IDisposable _disposable;
    private INetworkDamageService _damageService;
    private readonly Collider[] _results = new Collider[128];

    public override event Action<Projectile> OnReturnToPool;

    public override void Initialize()
    {
      if (!IsServerStarted)
        return;
      
      _disposable?.Dispose();
      var throwDirection = transform.forward;
      _rb.AddForce(throwDirection * ProjectileData.Speed, ForceMode.Impulse);
      _disposable = Observable.Timer(TimeSpan.FromSeconds(ProjectileData.ExplosionDelay))
        .Subscribe(_ => TakeDamage()).AddTo(this);
    }

    private void TakeDamage()
    {
      if (!IsServerStarted)
        return;
        
      int count = Physics.OverlapSphereNonAlloc(transform.position, ProjectileData.ExplosionRadius, _results);

      for (int i = 0; i < count; i++)
        if (_results[i].TryGetComponent(out IDamageable damageable))
          _damageService.SendDamageToEnemy(new DamageData(damageable.ActorNumber.Value, ProjectileData.Damage));

      OnReturnToPool?.Invoke(this);
    }
  }
}