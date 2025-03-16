using System;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.health
{
  public class Health : MonoBehaviour, IDamageable, IHealeable
  {
    private readonly ReactiveProperty<float> _health = new(100f);
    public IReadOnlyReactiveProperty<float> HealthPoints => _health;
    
    public event Action OnHealthOver;

    public void Construct(float initialHealth) => 
      _health.Value = Mathf.Clamp(initialHealth, 0, int.MaxValue);

    public void TakeDamage(float damage)
    {
      _health.Value = Mathf.Clamp(HealthPoints.Value - damage, 0, int.MaxValue);
      if (HealthPoints.Value <= 0) 
        OnHealthOver?.Invoke();
    }

    public void Heal(float value) => 
      _health.Value += value;
  }

  public interface IHealeable
  {
    void Heal(float value);
  }

  public interface IDamageable
  {
    void TakeDamage(float damage);
  }
}