using _Scripts.Gameplay.health.UI;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.Installers
{
  public class HudInstaller : MonoInstaller
  {
    [SerializeField] private HealthPresenter _healthPresenter;
    
    public override void Register(IContainerBuilder builder)
    {
      builder.RegisterInstance<IHealthPresenter>(_healthPresenter);
    }
  }
}