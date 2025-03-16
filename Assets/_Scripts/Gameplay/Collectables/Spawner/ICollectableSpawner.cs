using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Spawner
{
  public interface ICollectableSpawner
  {
    void Initialize();
    UniTask<Collectable> SpawnCollectable(CollectableType type, Vector3 at, Quaternion rotation);
  }
}