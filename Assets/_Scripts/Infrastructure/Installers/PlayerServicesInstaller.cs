using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.PlayerCamera.Services;
using _Scripts.Infrastructure.Services.Player;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Installers
{
  public class PlayerServicesInstaller : MonoInstaller
  {
    public override void Register(IContainerBuilder builder)
    {
      builder.Register<IPlayerServices, PlayerServices>(Lifetime.Singleton).As<ITickable>();
      builder.Register<IPlayerMover, PlayerMover>(Lifetime.Singleton);
      builder.Register<ICameraController, CameraController>(Lifetime.Singleton);
      builder.Register<ICameraFollower, CameraFollower>(Lifetime.Singleton);
      builder.Register<ICameraProvider, CameraProvider>(Lifetime.Singleton);
      builder.Register<IPlayerHealth, PlayerHealth>(Lifetime.Singleton);
      builder.Register<IPlayerBackpack, PlayerBackpack>(Lifetime.Singleton);
      builder.Register<IPlayerAttacker, PlayerAttacker>(Lifetime.Singleton);
      builder.Register<IPlayerAttackController, PlayerAttackController>(Lifetime.Singleton);
      builder.Register<PlayerInventory>(Lifetime.Singleton);
    }
  }
}