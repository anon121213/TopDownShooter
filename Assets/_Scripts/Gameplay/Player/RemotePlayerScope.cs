using _Scripts.Gameplay.Player.Spawner;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class RemotePlayerScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.Register<IRemotePlayerFactory, RemotePlayerFactory>(Lifetime.Singleton).As<IInitializable>();
      builder.RegisterEntryPoint<RemotePlayerEntryPoint>();
    }
  }
}