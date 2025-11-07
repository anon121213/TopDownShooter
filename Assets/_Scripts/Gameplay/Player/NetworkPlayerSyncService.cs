using _Scripts.Infrastructure.Scopes.NetCore;
using FishNet.Managing;
using FishNet.Object;
using VContainer;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player
{
  public class NetworkPlayerSyncService : NetworkBehaviour, INetworkPlayerSyncService, IInitializable
  {
    [Inject] private INetworkRoomModel _networkRoomModel;

    [Inject] private NetworkManager _networkManager;

    public void Initialize() => 
      _networkManager.ServerManager.Spawn(this);

    [ObserversRpc]
    public void SyncPlayerState(int playerId, PlayerStateDTO playerState)
    {
      foreach (var pair in _networkRoomModel.PlayersRoot)
      {
        if (pair.Value.PlayerId.Value != playerId) 
          continue;
        
        pair.Value.Apply(playerState);
        return;
      }
    }

    [ObserversRpc]
    public void SetIsDead(bool isDead)
    {
      
    }

    [ObserversRpc]
    public void SetHealth(float health)
    {
      
    }
  }

  public interface INetworkPlayerSyncService
  {
    void SyncPlayerState(int playerId, PlayerStateDTO playerState);
  }
}