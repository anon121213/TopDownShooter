using _Scripts.Gameplay.Player.Factory;
using _Scripts.Gameplay.PlayerCamera.Factory;
using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Player.Spawner
{
  public class PlayerSpawner : IPlayerSpawner
  {
    private readonly IPlayerFactory _playerFactory;
    private readonly ICameraFactory _cameraFactory;

    public PlayerSpawner(IPlayerFactory playerFactory,
      ICameraFactory cameraFactory)
    {
      _playerFactory = playerFactory;
      _cameraFactory = cameraFactory;
    }

    public async UniTask<Player> SpawnPlayer()
    {
      Player player = await _playerFactory.CreatePlayer();
      _cameraFactory.CreateCamera(player.transform);
      return player;
    }
  }
}