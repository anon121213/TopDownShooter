using _Scripts.Infrastructure.Services.Warmup;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.Game
{
  public class GameScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.Register<IWarmupService, WarmupService>(Lifetime.Singleton);
      builder.RegisterEntryPoint<GameService>();
    }
  }
}