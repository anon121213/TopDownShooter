using _Scripts.Gameplay.Collectables.UI;
using _Scripts.Gameplay.health.UI;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.Installers
{
  public class HudInstaller : MonoInstaller
  {
    [SerializeField] private HealthPresenter _healthPresenter;
    [SerializeField] private CoinsPresenter _coinsPresenter;
    
    public override void Register(IContainerBuilder builder)
    {
      builder.RegisterInstance<IHealthPresenter>(_healthPresenter);
      builder.RegisterInstance<ICoinsPresenter>(_coinsPresenter);
    }
  }
}