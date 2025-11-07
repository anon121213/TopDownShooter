using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.Game
{
  public class GameScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterEntryPoint<GameService>();
    }
  }
}