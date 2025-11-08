using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Player.Services.Base;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerCollector : PlayerService, IPlayerCollector
  {
    private readonly ReactiveProperty<int> _points = new();
    public IReadOnlyReactiveProperty<int> Points => _points;

    private bool _enabled;

    public override void OnInitialize()
    {
      PlayerRoot.PlayerCollider.OnTriggerEnterAsObservable()
        .Subscribe(OnCollide)
        .AddTo(Disposables);
    }

    public void OnCollide(Collider other)
    {
      if (!_enabled)
        return;

      if (!other.TryGetComponent(out Collectable collectable))
        return;
      
      _points.Value += collectable.Points;
      collectable.Claim();
    }
  }

  public interface IPlayerCollector
  {
    IReadOnlyReactiveProperty<int> Points { get; }
    void OnCollide(Collider other);
  }
}