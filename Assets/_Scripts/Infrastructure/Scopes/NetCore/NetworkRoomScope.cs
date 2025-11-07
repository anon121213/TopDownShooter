using _Scripts.Infrastructure.Scopes.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomScope : LifetimeScope
  {
    [SerializeField] private GameScope _gameScope;
    [SerializeField] private NetworkSyncService _networkSyncService;

    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterComponent(_networkSyncService).AsImplementedInterfaces();

      builder.Register<NetworkRoomModel>(Lifetime.Singleton)
        .As<INetworkRoomModel>()
        .As<IReadOnlyNetworkRoomModel>();

      builder.RegisterEntryPoint<NetworkRoomService>()
        .WithParameter(_gameScope);
    }
  }
}