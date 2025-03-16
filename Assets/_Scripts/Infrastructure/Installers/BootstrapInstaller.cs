using _Scripts.Infrastructure.Bootstrapper;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Installers
{
  public class BootstrapInstaller : MonoInstaller
  {
    [SerializeField] private bool _isGameInstaller;
    
    public override void Register(IContainerBuilder builder)
    {
      if (_isGameInstaller)
      {
        builder.RegisterEntryPoint<GameEntryPoint>().AsSelf();
        return;
      }
      
      builder.RegisterEntryPoint<MainBootstrapper>().AsSelf();
    }
  }
}