using System.Threading;
using _Scripts.Infrastructure.Constants;
using _Scripts.Infrastructure.Scopes;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Bootstrapper
{
  public class MainBootstrapper : IAsyncStartable
  {
    private readonly RootScope _rootScope;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly NetworkRoomScope _networkRoomScope;

    public MainBootstrapper(RootScope rootScope, IStaticDataProvider staticDataProvider, NetworkRoomScope networkRoomScope)
    {
      _rootScope = rootScope;
      _staticDataProvider = staticDataProvider;
      _networkRoomScope = networkRoomScope;
    }

    public async Awaitable StartAsync(CancellationToken cancellation = new())
    {
      Application.targetFrameRate = GameConstants.FRAMERATE;
      await _staticDataProvider.Initialize(cancellation);
      _rootScope.CreateChildFromPrefab(_networkRoomScope);
    }
  }
}