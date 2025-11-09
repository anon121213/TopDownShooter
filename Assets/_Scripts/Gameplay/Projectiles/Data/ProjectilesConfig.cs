using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Gameplay.Projectiles.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Projectiles/ProjectilesConfig", fileName = "ProjectilesConfig")]
  public class ProjectilesConfig : ScriptableObject
  {
    [SerializeField] private List<ProjectileData> _projectiles = new();

    public IReadOnlyList<ProjectileData> Projectiles => _projectiles;

    public bool TryGetProjectile(ProjectileTypeEnum projectileTypeEnum, out ProjectileData projectileData)
    {
      projectileData = _projectiles.FirstOrDefault(projectile => projectile.projectileTypeEnum == projectileTypeEnum);

      if (projectileData != null)
        return true;
      
      Debug.LogError($"Projectile {projectileTypeEnum} does not exist");
      return false;
    }
  }

  [Serializable]
  public class ProjectileData
  {
    public ProjectileTypeEnum projectileTypeEnum;
    public Projectile Prefab;
    public float Speed;
    public float Damage;
    public float ExplosionRadius;
    public float ExplosionDelay;
  }

  public enum ProjectileTypeEnum
  {
    None = 0,
    Bullet = 1,
    Granade = 2
  }
}