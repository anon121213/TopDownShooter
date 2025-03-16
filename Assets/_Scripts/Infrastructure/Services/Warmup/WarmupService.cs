using System.Collections.Generic;
using _Scripts.Gameplay.Collectables.Factory;
using _Scripts.Gameplay.Enemies.Factory;
using _Scripts.Gameplay.Items.Weapons.Factory;
using _Scripts.Gameplay.Player.Factory;
using _Scripts.Gameplay.PlayerCamera.Factory;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Warmup
{
  public class WarmupService : IWarmupService
  {
    private readonly List<IWarmupable> _warmupables = new();

    public WarmupService(IStaticDataProvider staticDataProvider,
      IPlayerFactory playerFactory,
      IEnemyFactory enemyFactory,
      ICameraFactory cameraFactory,
      IWeaponFactory weaponFactory,
      ICollectableFactory collectableFactory)
    {
      _warmupables.Add(staticDataProvider);
      _warmupables.Add(playerFactory);
      _warmupables.Add(cameraFactory);
      _warmupables.Add(enemyFactory);
      _warmupables.Add(weaponFactory);
      _warmupables.Add(collectableFactory);
    }

    public async UniTask Warmup()
    {
      foreach (var warmupable in _warmupables)
        await warmupable.Warmup();
    }
  }

  public interface IWarmupable
  {
    UniTask Warmup();
  }
}