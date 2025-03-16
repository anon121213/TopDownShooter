using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.UI;
using _Scripts.Infrastructure.Services.Player;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerCollecter : IPlayerCollecter
  {
    private readonly ICoinsPresenter _coinsPresenter;
    private readonly ReactiveProperty<int> _points = new();
    public IReadOnlyReactiveProperty<int> Points => _points;

    private bool _enabled;

    public PlayerCollecter(ICoinsPresenter coinsPresenter) => 
      _coinsPresenter = coinsPresenter;

    public void Initialize() => 
      _coinsPresenter.Initialize();

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

  public interface IPlayerCollecter : IPlayerService, IInitializable
  {
    IReadOnlyReactiveProperty<int> Points { get; }
    void OnCollide(Collider other);
  }
}