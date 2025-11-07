using _Scripts.Gameplay.health;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Gameplay.PlayerCamera.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class PlayerScope : LifetimeScope
  {
    [SerializeField] private PlayerView _player;
    [SerializeField] private NetworkPlayerSyncService _playerSyncService;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterComponent(_player).AsSelf();
      builder.RegisterComponent<INetworkPlayerSyncService>(_playerSyncService);

      builder.Register<PlayerModel>(Lifetime.Scoped).As<IPlayerModel>().As<IReadOnlyPlayerModel>();
      builder.Register<IPlayerServices, PlayerServices>(Lifetime.Scoped).As<ITickable>();
      builder.Register<IPlayerMover, PlayerMover>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<IPlayerHealth, PlayerHealth>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<IPlayerAttacker, PlayerAttacker>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<IPlayerAttackController, PlayerAttackController>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<IPlayerCollector, PlayerCollector>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<PlayerInventory>(Lifetime.Scoped).As<IPlayerService>();
      builder.Register<IPlayerBackpack, PlayerBackpack>(Lifetime.Scoped);
      // TODO MAKE ABSTRACT
      builder.Register<IWeaponFactory, WeaponFactory>(Lifetime.Singleton);
      builder.Register<ICameraFactory, CameraFactory>(Lifetime.Singleton);
      builder.Register<ICameraProvider, CameraProvider>(Lifetime.Scoped);
      builder.Register<ICameraFollower, CameraFollower>(Lifetime.Scoped);
      builder.Register<ICameraController, CameraController>(Lifetime.Scoped);
      
      builder.RegisterEntryPoint<PlayerEntryPoint>();
    }
  }
}