using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.health.UI
{
  public class HealthPresenter : MonoBehaviour, IHealthPresenter
  { 
    [SerializeField] private HealthView _healthView;
    private Health _health;

    public void Initialize(Health health)
    {
      _health = health;
      _health.HealthPoints.Subscribe(value => _healthView.SetHealth(value)).AddTo(this);
    }
  }

  public interface IHealthPresenter
  {
    void Initialize(Health health);
  }
}