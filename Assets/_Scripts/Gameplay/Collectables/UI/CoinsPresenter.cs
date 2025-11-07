using _Scripts.Gameplay.Player.Services;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Collectables.UI
{
  public class CoinsPresenter : MonoBehaviour, IInitializable
  {
    [SerializeField] private CoinsView _coinsView;
    [Inject] private IPlayerCollector _collector;

    public void Initialize() => 
      _collector.Points.Subscribe(value => 
        _coinsView.ChangeCoinsValue(value)).AddTo(this);
  }
}