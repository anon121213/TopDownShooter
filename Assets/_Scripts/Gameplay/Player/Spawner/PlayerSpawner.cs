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

    public void SpawnPlayer(int actorNumber)
    {
      if (!_networkRoomModel.IsServer.Value)
        return;
  
      _networkRoomModel.AddDtoPlayer(new PlayerModelDTO(
        actorNumber,
        false,
        _playerConfig.InitHealth
      ));
    }

    public void DespawnPlayer(int actorNumber) => 
      _networkRoomModel.RemovePlayer(actorNumber);
  }

  public interface IPlayerSpawner
  {
    void SpawnPlayer(int actorNumber);
    void DespawnPlayer(int actorNumber);
  }
}