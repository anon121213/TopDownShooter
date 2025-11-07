using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Player.Services.Base;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerCollector : IPlayerCollector
  {
    private readonly ReactiveProperty<int> _points = new();
    public IReadOnlyReactiveProperty<int> Points => _points;

    private bool _enabled;

    public void Enable() => 
      _enabled = true;

    public void OnCollide(Collider other)
    {
      if (!_enabled)
        return;

      if (!other.TryGetComponent(out Collectable collectable))
        return;
      
      _points.Value += collectable.Points;
      collectable.Claim();
    }

    public void Disable() => 
      _enabled = false;
  }

  public interface IPlayerCollector : IPlayerService
  {
    IReadOnlyReactiveProperty<int> Points { get; }
    void OnCollide(Collider other);
  }
}