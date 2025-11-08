using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Gameplay.PlayerCamera.Services;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class PlayerScope : LifetimeScope
  {
    protected override void Configure(IContainerBuilder builder)
    {
      builder.Register<IPlayerServices, PlayerServices>(Lifetime.Singleton).As<ITickable>();
      builder.Register<IPlayerMover, PlayerMover>(Lifetime.Singleton).As<PlayerService>();
      builder.Register<IPlayerAttacker, PlayerAttacker>(Lifetime.Singleton).As<PlayerService>();
      builder.Register<IPlayerAttackController, PlayerAttackController>(Lifetime.Scoped).As<PlayerService>();
      builder.Register<IPlayerCollector, PlayerCollector>(Lifetime.Singleton).As<PlayerService>();
      builder.Register<PlayerInventory>(Lifetime.Singleton).As<PlayerService>();
      builder.Register<IPlayerBackpack, PlayerBackpack>(Lifetime.Singleton);
      // TODO MAKE ABSTRACT
      builder.Register<IWeaponFactory, WeaponFactory>(Lifetime.Singleton);
      builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton).As<IInitializable>();
      builder.Register<ICameraFactory, CameraFactory>(Lifetime.Singleton);
      builder.Register<ICameraProvider, CameraProvider>(Lifetime.Singleton);
      builder.Register<ICameraFollower, CameraFollower>(Lifetime.Singleton);
      builder.Register<ICameraController, CameraController>(Lifetime.Singleton);
      
      builder.RegisterEntryPoint<PlayerEntryPoint>();
    }
  }
}