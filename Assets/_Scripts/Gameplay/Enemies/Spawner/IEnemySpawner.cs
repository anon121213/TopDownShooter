using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public interface IEnemySpawner
  {
    UniTask<List<SimpleEnemy>> CreateSimpleEnemiesOnSpawnPoints();
  }
}