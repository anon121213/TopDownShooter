using _Scripts.Infrastructure.Bootstrapper;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Installers
{
  public class BootstrapInstaller : MonoInstaller
  {
    public override void Register(IContainerBuilder builder)
    {
      builder.RegisterEntryPoint<MainBootstrapper>().AsSelf();
    }
  }
}