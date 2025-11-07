using System;
using _Scripts.Gameplay.Items.Base;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Gameplay.Player.Services;
using _Scripts.Gameplay.Player.Services.Base;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Managing;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using IInitializable = VContainer.Unity.IInitializable;

namespace _Scripts.Gameplay.Player
{
  public class PlayerEntryPoint : IInitializable, IDisposable
  {
    private readonly PlayerView _player;
    private readonly IPlayerServices _services;
    private readonly IPlayerCollector _collector;
    private readonly ICameraFactory _cameraFactory;
    private readonly IReadOnlyNetworkRoomModel _networkRoomModel;
    private readonly INetworkSyncService _networkSyncService;
    private readonly INetworkPlayerSyncService _playerSyncService;
    private readonly IWeaponFactory _weaponFactory;
    private readonly IPlayerAttacker _playerAttacker;
    private readonly IPlayerBackpack _playerBackpack;

    private readonly CompositeDisposable _disposables = new();
    private readonly IPlayerModel _playerModel;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly NetworkManager _networkManager;

    public PlayerEntryPoint(PlayerView player, IPlayerServices services, IPlayerCollector collector,
      IWeaponFactory weaponFactory, IPlayerAttacker playerAttacker, IPlayerBackpack playerBackpack,
      ICameraFactory cameraFactory, IReadOnlyNetworkRoomModel networkRoomModel, INetworkSyncService networkSyncService,
      INetworkPlayerSyncService playerSyncService, IPlayerModel playerModel, IStaticDataProvider staticDataProvider,
      NetworkManager networkManager)
    {
      _player = player;
      _services = services;
      _collector = collector;
      _cameraFactory = cameraFactory;
      _networkRoomModel = networkRoomModel;
      _networkSyncService = networkSyncService;
      _playerSyncService = playerSyncService;
      _playerModel = playerModel;
      _staticDataProvider = staticDataProvider;
      _networkManager = networkManager;
      _weaponFactory = weaponFactory;
      _playerAttacker = playerAttacker;
      _playerBackpack = playerBackpack;
    }

    public void Initialize()
    {
      _networkManager.ServerManager.Spawn((NetworkPlayerSyncService)_playerSyncService); // TODO MOVE TO SPAWNER BY SERVER 
      Debug.LogError(((NetworkPlayerSyncService)_playerSyncService).IsSpawned);
      var config = _staticDataProvider.GetConfig<PlayerConfig>();
      _playerModel.SetConfig(config);
      
      _playerSyncService.SyncPlayerState(_networkRoomModel.ClientId.Value, new PlayerStateDTO(
        true,
        false,
        _playerModel.PlayerConfig.HealthConfig.InitHealth
      ));
      
      _networkSyncService.AddPlayer(_networkRoomModel.ClientId.Value);

      _cameraFactory.CreateCamera(_player.transform);

      _player.Health.OnHealthOver += Die;
      _services.InitializeServices();
      _services.EnableServices();

      _player.PlayerCollider.OnTriggerEnterAsObservable()
        .Subscribe(_collector.OnCollide)
        .AddTo(_disposables);

      var pistol = _weaponFactory.CreateWeapon(ItemType.Pistol, _player.transform);
      var grenade = _weaponFactory.CreateWeapon(ItemType.Grenade, _player.transform);
      _playerBackpack.AddItem(pistol, 1);
      _playerBackpack.AddItem(grenade, 3);
      _playerAttacker.SwitchWeapon(pistol);
    }

    private void Die()
    {
      _services.DisableServices();
      Debug.LogError("YouDied");
    }

    public void Dispose()
    {
      _player.Health.OnHealthOver -= Die;
      _services.DisableServices();
      _disposables.Dispose();
    }
  }
}