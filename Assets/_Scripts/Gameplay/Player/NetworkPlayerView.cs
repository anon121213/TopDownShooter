using _Scripts.Gameplay.health;
using _Scripts.Infrastructure.Scopes.ArenaScene;
using _Scripts.Infrastructure.Scopes.NetCore;
using FishNet.Object;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class NetworkPlayerView : NetworkBehaviour, IDamageable
  {
    [field: SerializeField] public PlayerModel PlayerModel { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] private NetworkConfig _networkConfig;
    
    public IReadOnlyReactiveProperty<int> ActorNumber => PlayerModel.ActorNumber;
    
    public override void OnStartClient()
    {
      if (IsOwner)
      {
        LifetimeScope.Find<ArenaSceneScope>()
          .CreateChildFromPrefab(
            _networkConfig.LocalPlayerScopePrefab,
            builder => builder.RegisterInstance(this));
      }
      else
      {
        LifetimeScope.Find<ArenaSceneScope>()
          .CreateChildFromPrefab(
            _networkConfig.RemotePlayerScopePrefab,
            builder => builder.RegisterInstance(this));
      }
    }
  }
}