using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Player.Spawner
{
  public interface IPlayerSpawner
  {
    UniTask<Player> SpawnPlayer();
  }
}