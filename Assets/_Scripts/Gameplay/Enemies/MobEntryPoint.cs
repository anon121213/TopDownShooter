using System;
using _Scripts.Gameplay.Enemies.Base;
using _Scripts.Gameplay.Enemies.Data;
using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Managing;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Enemies
{
  public class MobEntryPoint : IInitializable, IDisposable
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IEnemyFactory _enemyFactory;
    private readonly INetworkRoomModel _networkRoomModel;
    private readonly MobModelDataDTO _modelDataDto;
    private readonly NetworkManager _networkManager;
    private readonly IArenaSceneModel _arenaSceneModel;
    private readonly LocalMobScope _localMobScope;
    private readonly CompositeDisposable _disposables = new();

    private Enemy _enemy;
    private EnemyData _config;

    public MobEntryPoint(IStaticDataProvider staticDataProvider, IEnemyFactory enemyFactory,
      INetworkRoomModel networkRoomModel, MobModelDataDTO modelDataDto, NetworkManager networkManager,
      IArenaSceneModel arenaSceneModel, LocalMobScope localMobScope)
    {
      _staticDataProvider = staticDataProvider;
      _enemyFactory = enemyFactory;
      _networkRoomModel = networkRoomModel;
      _modelDataDto = modelDataDto;
      _networkManager = networkManager;
      _arenaSceneModel = arenaSceneModel;
      _localMobScope = localMobScope;
    }

    public void Initialize()
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
      
      var enemiesConfig = _staticDataProvider.GetConfig<EnemiesConfig>();

      if (!enemiesConfig.TryGetConfigByType(_modelDataDto.MobType, out var config))
        return;

      _enemy = _enemyFactory.CreateEnemyByType(_modelDataDto.MobType, _modelDataDto.SpawnPosition, Quaternion.identity);
      
      if (_networkRoomModel.IsServer.Value)
        _networkManager.ServerManager.Spawn(_enemy);
      
      _enemy.MobModel.Apply(_modelDataDto);
      _enemy.SetContext(new Context(config));

      _arenaSceneModel.AddMob(_enemy);
      _arenaSceneModel.AddMobScope(_enemy.MobModel.ActorNumber.Value, _localMobScope);

      Observable.Interval(TimeSpan.FromSeconds(0.5f))
        .Subscribe(_ =>
        {
          if (_networkRoomModel.IsServer.Value && !_enemy.IsPooled.Value && !_enemy.IsEnabled.Value && !_enemy.MobModel.IsDead.Value)
            _enemy.EnableEnemy();
          else if (!_networkRoomModel.IsServer.Value && !_enemy.IsPooled.Value && _enemy.IsEnabled.Value && !_enemy.MobModel.IsDead.Value)
            _enemy.DisableEnemy();
        }).AddTo(_disposables);

      _enemy.MobModel.IsDead
        .Delay(TimeSpan.FromSeconds(config.DestroyBeforeDeathDelay))
        .Subscribe(isDead =>
        {
          if (!isDead || !_networkRoomModel.IsServer.Value)
            return;

          _networkRoomModel.RemoveMob(_enemy.MobModel.ActorNumber.Value);
        }).AddTo(_disposables);

      _enemy.IsEnabled
        .StartWith(_enemy.IsEnabled.Value)
        .Subscribe(val =>
        {
          if (!_networkRoomModel.IsServer.Value)
            return;

          _enemy.MobModel.SetIsEnable(val);
        })
        .AddTo(_disposables);

      if (_enemy is IPlayerTargetableEnemy targetableEnemy)
      {
        foreach (var player in _arenaSceneModel.Players)
          targetableEnemy.TryAddTarget(player.Value.transform);

        _arenaSceneModel.Players
          .ObserveAdd()
          .Subscribe(player =>
            targetableEnemy.TryAddTarget(player.Value.transform))
          .AddTo(_disposables);

        _arenaSceneModel.Players
          .ObserveRemove()
          .Subscribe(player =>
            targetableEnemy.TryRemoveTarget(player.Value.transform))
          .AddTo(_disposables);
      }

      if (_networkRoomModel.IsServer.Value) _enemy.EnableEnemy();
      else _enemy.DisableEnemy();
    }

    public void Dispose()
    {
      _disposables?.Dispose();
      _arenaSceneModel.RemoveMob(_enemy.MobModel.ActorNumber.Value);
      _arenaSceneModel.RemoveMobScope(_enemy.MobModel.ActorNumber.Value);
    }
  }
}