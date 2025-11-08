using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Pool;
using UnityEngine;
using IInitializable = VContainer.Unity.IInitializable;

namespace _Scripts.Gameplay.Player.Spawner
{
  public class PlayerFactory : IInitializable, IPlayerFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IWeaponFactory _weaponFactory;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IObjectPool _objectPool;
    private readonly ICameraFactory _cameraFactory;
    private PlayerConfig _playerConfig;

    public PlayerFactory(IStaticDataProvider staticDataProvider, IWeaponFactory weaponFactory,
      IPlayerBackpack playerBackpack, IPlayerAttacker playerAttacker,
      IObjectPool objectPool, ICameraFactory cameraFactory)
    {
      _staticDataProvider = staticDataProvider;
      _weaponFactory = weaponFactory;
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
      _objectPool = objectPool;
      _cameraFactory = cameraFactory;
    }

    public void Initialize()
    {
      _playerConfig = _staticDataProvider.GetConfig<PlayerConfig>();
    }

    public PlayerRootView CreatePlayer(Vector3 position, Quaternion rotation)
    {
      var player = _objectPool.GetGameObject(_playerConfig.Prefab, position, rotation);
      
      var pistol = _weaponFactory.CreateWeapon(ItemType.Pistol, player.transform);
      var grenade = _weaponFactory.CreateWeapon(ItemType.Grenade, player.transform);
      
      _playerBackpack.AddItem(pistol, 1);
      _playerBackpack.AddItem(grenade, 3);
      
      _playerAttacker.SwitchWeapon(pistol);
      _cameraFactory.CreateCamera(player.transform);
      return player;
    }

    public void ReturnPlayer(PlayerRootView playerRoot) => 
      _objectPool.ReturnGameObject(playerRoot, _playerConfig.Prefab);
  }

  public interface IPlayerFactory
  {
    PlayerRootView CreatePlayer(Vector3 position, Quaternion rotation);
    void ReturnPlayer(PlayerRootView playerRoot);
  }
}