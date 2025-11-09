using _Scripts.Infrastructure.Scopes.Game;
using FishNet.Managing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  public class NetworkRoomScope : LifetimeScope
  {
    [SerializeField] private GameScope _gameScope;
    [SerializeField] private NetworkRoomModel _networkRoomModelPrefab;

    [Inject] private NetworkManager _networkManager;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterEntryPoint<NetworkRoomService>().WithParameter(_gameScope).WithParameter(_networkRoomModelPrefab);
    }
  }
}