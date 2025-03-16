using _Scripts.Gameplay.Player.Services;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.Collectables.UI
{
  public class CoinsPresenter : MonoBehaviour, ICoinsPresenter
  {
    [SerializeField] private CoinsView _coinsView;
    private IPlayerCollecter _collecter;

    [Inject]
    private void Construct(IPlayerCollecter collecter) => 
      _collecter = collecter;

    public void Initialize() => 
      _collecter.Points.Subscribe(value => 
        _coinsView.ChangeCoinsValue(value)).AddTo(this);
  }

  public interface ICoinsPresenter
  {
    void Initialize();
  }
}