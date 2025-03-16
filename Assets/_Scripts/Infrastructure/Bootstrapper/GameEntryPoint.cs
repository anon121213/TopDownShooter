using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player;
using _Scripts.Gameplay.Player.Factory;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Warmup;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Bootstrapper
{
  public class GameEntryPoint : IInitializable
  {
    private readonly IAssetProvider _assetProvider;
    private readonly IWarmupService _warmupService;
    private readonly IPlayerFactory _playerFactory;
    private readonly ICameraFactory _cameraFactory;
    private readonly IEnemySpawner _enemySpawner;
    private readonly IWeaponFactory _weaponFactory;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IPlayerBackpack _playerBackpack;

    public GameEntryPoint(IAssetProvider assetProvider,
      IWarmupService warmupService,
      IPlayerFactory playerFactory,
      ICameraFactory cameraFactory,
      IEnemySpawner enemySpawner,
      IWeaponFactory weaponFactory,
      IPlayerAttacker playerAttacker,
      IPlayerBackpack playerBackpack)
    {
      _assetProvider = assetProvider;
      _warmupService = warmupService;
      _playerFactory = playerFactory;
      _cameraFactory = cameraFactory;
      _enemySpawner = enemySpawner;
      _weaponFactory = weaponFactory;
      _playerAttacker = playerAttacker;
      _playerBackpack = playerBackpack;
    }

    public async void Initialize()
    {
      await _warmupService.Warmup();
      
      Player player = await _playerFactory.CreatePlayer();
      _cameraFactory.CreateCamera(player.transform);
      await _enemySpawner.CreateSimpleEnemiesOnSpawnPoints();

      var weapon = _weaponFactory.CreateWeapon(ItemType.Pistol);
      _playerAttacker.SwitchWeapon(weapon);
      _playerBackpack.AddItem(weapon, 1);
      
      player.Initialize();
      _assetProvider.Cleanup();
    }
  }
}