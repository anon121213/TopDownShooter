using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Enemies
{
  public class MobScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterEntryPoint<MobEntryPoint>();
    }
  }
}