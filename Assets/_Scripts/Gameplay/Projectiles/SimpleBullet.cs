using System;
using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Projectiles.Data;
using _Scripts.Infrastructure.Services.Network;
using FishNet.Object;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles
{
  public class SimpleBullet : Projectile
  {
    private float _damage;
    private float _speed;
    private bool _isCollided;
    
    private float _lifeTime;
    private const float MAX_LIFETIME = 5f;

    private INetworkDamageService _networkDamageService;
    
    public override event Action<Projectile> OnReturnToPool;

    public override void Construct(ProjectileData projectileData, INetworkDamageService damageService)
    {
      _damage = projectileData.Damage;
      _speed = projectileData.Speed;
      _networkDamageService = damageService;
      _lifeTime = MAX_LIFETIME;
    }

    public override void Initialize() => 
      _isCollided = false;

    private void Update()
    {
      if (_isCollided || !IsServerStarted)
        return;

      transform.position += transform.forward * (_speed * Time.deltaTime);
      
      _lifeTime -= Time.deltaTime;
      if (_lifeTime <= 0f) 
        OnReturnToPool?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!IsServerStarted)
        return;
      
      if (other.TryGetComponent(out IDamageable damageable))
        _networkDamageService.SendDamageToEnemy(new DamageData(damageable.ActorNumber.Value, _damage));

      OnReturnToPool?.Invoke(this);
      _isCollided = true;
    }
  }

  public abstract class Projectile : NetworkBehaviour
  {
    public int ActorNumber { get; private set; }

    public abstract void Initialize();
    public abstract event Action<Projectile> OnReturnToPool;
    public abstract void Construct(ProjectileData projectileData, INetworkDamageService networkDamageService);
    public void SetActorNumber(int actorNumber) => ActorNumber = actorNumber;
  }
}