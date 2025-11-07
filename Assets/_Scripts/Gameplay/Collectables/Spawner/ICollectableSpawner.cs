using System.Threading;
using _Scripts.Gameplay.Collectables.Base;
using _Scripts.Gameplay.Collectables.Data;
using _Scripts.Infrastructure.Services.Warmup;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Spawner
{
  public interface ICollectableSpawner : IWarmupable
  {
    Collectable SpawnCollectable(CollectableType type, Vector3 at, Quaternion rotation, CancellationToken ct);
  }
}