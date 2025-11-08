using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player.Spawner
{
  public class PlayerSpawner : IPlayerSpawner, IInitializable 
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly INetworkRoomModel _networkRoomModel;

    private PlayerConfig _playerConfig;
    
    public PlayerSpawner(IStaticDataProvider staticDataProvider,
      INetworkRoomModel networkRoomModel)
    {
      _staticDataProvider = staticDataProvider;
      _networkRoomModel = networkRoomModel;
    }

    public void Initialize() => 
      _playerConfig = _staticDataProvider.GetConfig<PlayerConfig>();

    public void SpawnLocalPlayer()
    {
      if (!_networkRoomModel.IsServer.Value)
        return;

      _networkRoomModel.AddDtoPlayer(new PlayerStateDTO(
        _networkRoomModel.ClientId.Value,
        true,
        false,
        _playerConfig.InitHealth
      ));
    }
  }

  public interface IPlayerSpawner
  {
    void SpawnLocalPlayer();
  }
}