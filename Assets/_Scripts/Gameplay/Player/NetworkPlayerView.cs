using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using FishNet.Object;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class NetworkPlayerView : NetworkBehaviour
  {
    [field: SerializeField] public PlayerModel PlayerModel { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] private NetworkConfig _networkConfig;
    
    public IReadOnlyReactiveProperty<int> ActorNumber => PlayerModel.ActorNumber;
    
    public override void OnStartClient()
    {
      if (IsOwner)
      {
        LifetimeScope scope = null;
        Observable.EveryUpdate().TakeWhile(_ => scope == null).Subscribe(_ =>
        {
          scope = LifetimeScope.Find<ArenaSceneScope>();
          
          scope?.CreateChildFromPrefab(
            _networkConfig.LocalPlayerScopePrefab,
            builder => builder.RegisterInstance(this));
        }).AddTo(this);
      }
      else
      {
        LifetimeScope scope = null;
        Observable.EveryUpdate().TakeWhile(_ => scope == null).Subscribe(_ =>
        {
          scope = LifetimeScope.Find<ArenaSceneScope>();
          
          scope?.CreateChildFromPrefab(
            _networkConfig.RemotePlayerScopePrefab,
            builder => builder.RegisterInstance(this));
        }).AddTo(this);
      }
    }
  }
}