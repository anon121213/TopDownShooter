using _Scripts.Gameplay.Collectables.Spawner;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Warmup;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Bootstrapper
{
  public class GameEntryPoint : IInitializable
  {
    private readonly IAssetProvider _assetProvider;
    private readonly IWarmupService _warmupService;
    private readonly IEnemySpawner _enemySpawner;
    private readonly IWeaponFactory _weaponFactory;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerSpawner _playerSpawner;
    private readonly ICollectableSpawner _collectableSpawner;

    public GameEntryPoint(IAssetProvider assetProvider,
      IWarmupService warmupService,
      IEnemySpawner enemySpawner,
      IWeaponFactory weaponFactory,
      IPlayerAttacker playerAttacker,
      IPlayerBackpack playerBackpack,
      IPlayerSpawner playerSpawner,
      ICollectableSpawner collectableSpawner)
    {
      _assetProvider = assetProvider;
      _warmupService = warmupService;
      _enemySpawner = enemySpawner;
      _weaponFactory = weaponFactory;
      _playerAttacker = playerAttacker;
      _playerBackpack = playerBackpack;
      _playerSpawner = playerSpawner;
      _collectableSpawner = collectableSpawner;
    }

    public async void Initialize()
    {
      await _warmupService.Warmup();

      var player = await _playerSpawner.SpawnPlayer();
      await _enemySpawner.CreateSimpleEnemiesOnSpawnPoints();

      var pistol = _weaponFactory.CreateWeapon(ItemType.Pistol, player.transform);
      var grenade = _weaponFactory.CreateWeapon(ItemType.Grenade, player.transform);
      _playerBackpack.AddItem(pistol, 1);
      _playerBackpack.AddItem(grenade, 3);
      _playerAttacker.SwitchWeapon(pistol);
      
      player.Initialize();
      _collectableSpawner.Initialize();
      _assetProvider.Cleanup();
    }
  }
}