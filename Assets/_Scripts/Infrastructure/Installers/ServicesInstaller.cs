using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Input;
using _Scripts.Infrastructure.Services.Pool;
using _Scripts.Infrastructure.Services.Scenes;
using _Scripts.Infrastructure.Services.Warmup;
using VContainer;

namespace _Scripts.Infrastructure.Installers
{
  public class ServicesInstaller : MonoInstaller
  {
    public override void Register(IContainerBuilder builder)
    {
      builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
      builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
      builder.Register<IStaticDataProvider, StaticDataProvider>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<IWarmupService, WarmupService>(Lifetime.Singleton);
      builder.Register<IInputService, InputService>(Lifetime.Singleton);
      builder.Register<IObjectPool, ObjectPool>(Lifetime.Singleton);
    }
  }
}