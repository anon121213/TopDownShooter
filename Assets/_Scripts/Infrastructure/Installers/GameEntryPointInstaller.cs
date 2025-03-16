using _Scripts.Infrastructure.Bootstrapper;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Installers
{
  public class GameEntryPointInstaller : MonoInstaller
  {
    public override void Register(IContainerBuilder builder)
    {
      builder.RegisterEntryPoint<GameEntryPoint>().AsSelf();
    }
  }
}