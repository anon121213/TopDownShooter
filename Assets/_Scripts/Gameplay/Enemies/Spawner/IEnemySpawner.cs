using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Scripts.Gameplay.Enemies.Spawner
{
  public interface IEnemySpawner
  {
    List<SimpleEnemy> CreateSimpleEnemiesOnSpawnPoints();
  }
}