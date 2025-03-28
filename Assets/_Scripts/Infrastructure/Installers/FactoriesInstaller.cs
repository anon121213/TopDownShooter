﻿using _Scripts.Gameplay.Collectables.Factory;
using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Factory;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Gameplay.Projectiles.Factory;
using _Scripts.Infrastructure.Services.Warmup;
using VContainer;

namespace _Scripts.Infrastructure.Installers
{
  public class FactoriesInstaller : MonoInstaller
  {
    
    public override void Register(IContainerBuilder builder)
    {
      builder.Register<IPlayerFactory, PlayerFactory>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<ICameraFactory, CameraFactory>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<IEnemyFactory, EnemyFactory>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<IWeaponFactory, WeaponFactory>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<ICollectableFactory, CollectableFactory>(Lifetime.Singleton).As<IWarmupable>();
      builder.Register<IProjectileFactory, ProjectileFactory>(Lifetime.Singleton);
    }
  }
}