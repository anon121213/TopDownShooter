using _Scripts.Gameplay.health;
using _Scripts.Gameplay.health.UI;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Player;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player.Factory
{
  public class PlayerFactory : IPlayerFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly IObjectResolver _objectResolver;
    private readonly IPlayerServices _playerServices;
    private readonly IPlayerMover _playerMover;
    private readonly IPlayerHealth _playerHealth;
    private readonly IHealthPresenter _healthPresenter;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IPlayerAttackController _attackController;
    private readonly PlayerInventory _playerInventory;
    private readonly IPlayerCollecter _collecter;

    private PlayerConfig _config;
    
    public PlayerFactory(IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      IObjectResolver objectResolver,
      IPlayerServices playerServices,
      IPlayerMover playerMover,
      IPlayerHealth playerHealth,
      IHealthPresenter healthPresenter,
      IPlayerAttacker playerAttacker,
      IPlayerAttackController attackController,
      PlayerInventory playerInventory,
      IPlayerCollecter collecter)
    {
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _objectResolver = objectResolver;
      _playerServices = playerServices;
      _playerMover = playerMover;
      _playerHealth = playerHealth;
      _healthPresenter = healthPresenter;
      _playerAttacker = playerAttacker;
      _attackController = attackController;
      _playerInventory = playerInventory;
      _collecter = collecter;
    }
      
    public async UniTask Warmup()
    {
      _config = _staticDataProvider.GetConfig<PlayerConfig>();
      await _assetProvider.LoadAssetAsync<Player>(_config.Prefab);
    }

    public async UniTask<Player> CreatePlayer()
    {
      Player prefab = await _assetProvider.LoadAssetAsync<Player>(_config.Prefab);
      Player player = _objectResolver.Instantiate(prefab, _config.SpawnPosition, Quaternion.identity);
      
      CreateMover(player);
      CreateHealth(player);
      CreateAttacker();
      CreateInventory();
      CreateCollecter();
      return player;
    }

    private void CreateMover(Player player)
    {
      _playerMover.Construct(player.CharacterController);
      _playerServices.AddService(_playerMover);
    }

    private void CreateHealth(Player player)
    {
      _playerHealth.Construct(player.Health, _healthPresenter, _config.HealthConfig);
      _playerServices.AddService(_playerHealth);
    }

    private void CreateAttacker()
    {
      _playerServices.AddService(_playerAttacker);
      _playerServices.AddService(_attackController);
    }

    private void CreateInventory()
    {
      _playerServices.AddService(_playerInventory);
    }

    private void CreateCollecter()
    {
      _playerServices.AddService(_collecter);
    }
  }

  public interface IPlayerFactory : IWarmupable
  {
    UniTask<Player> CreatePlayer();
  }
}