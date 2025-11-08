using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Enemies.Spawner;
using _Scripts.Gameplay.Player.Spawner;
using _Scripts.Infrastructure.Scopes.Game;
using _Scripts.Infrastructure.Services.Network;
using _Scripts.Infrastructure.Services.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomScope : LifetimeScope
  {
    [SerializeField] private GameScope _gameScope;
    [SerializeField] private NetworkRoomModel _networkRoomModelPrefab;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterInstance(Instantiate(_networkRoomModelPrefab)).As<IReadOnlyNetworkRoomModel>().As<INetworkRoomModel>();

      builder.Register<EnemyFactory>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<NetworkDamageService>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<ActorNumberAllocator>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<PlayerSpawner>(Lifetime.Singleton).AsImplementedInterfaces();
      builder.Register<EnemySpawner>(Lifetime.Singleton).AsImplementedInterfaces();
      
      builder.RegisterEntryPoint<NetworkRoomService>().WithParameter(_gameScope);
    }
  }
}