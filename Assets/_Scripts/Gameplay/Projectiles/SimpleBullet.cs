using System;
using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Projectiles.Data;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles
{
  public class SimpleBullet : Projectile
  {
    private float _damage;
    private float _speed;
    private bool _isCollided;

    public override event Action<Projectile> OnCollide;

    public override void Construct(ProjectileConfig projectileConfig)
    {
      _damage = projectileConfig.Damage;
      _speed = projectileConfig.Force;
    }

    public override void Initialize()
    {
      _isCollided = false;
    }

    private void Update()
    {
      if (_isCollided)
        return;

      transform.position += transform.forward * (_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out IDamageable damageable))
        damageable.TakeDamage(_damage);

      OnCollide?.Invoke(this);
      _isCollided = true;
    }
  }

  public abstract class Projectile : MonoBehaviour
  {
    public abstract void Initialize();
    public abstract event Action<Projectile> OnCollide;
    public abstract void Construct(ProjectileConfig projectileConfig);
  }
}