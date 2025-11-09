using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Enemies.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Enemies.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Enemies/EnemiesConfig", fileName = "EnemiesConfig")]
  public class EnemiesConfig : ScriptableObject
  {
    [SerializeField] private List<EnemyData> _enemiesData = new();

    public IReadOnlyList<EnemyData> EnemyData => _enemiesData;

    public bool TryGetConfigByType(MobType mobType, out EnemyData data)
    {
      data = _enemiesData.FirstOrDefault(data => data.MobType == mobType);

      if (data == null) 
        Debug.LogError("Mob does not present in EnemiesConfig!");
      
      return data != null;
    }
  }

  [Serializable]
  public class EnemyData
  {
    [field: SerializeField] public Enemy Prefab { get; private set; }
    [field: SerializeField] public MobType MobType { get; private set; }
    [field: SerializeField] public MobBehaviourTypes BehaviourType { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float AttackRadius { get; private set; }
    [field: SerializeField] public float AttackDelay { get; private set; }
    [field: SerializeField] public int MaxComboCount { get; private set; }
    [field: SerializeField] public float StartHealth { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float AngularSpeed { get; private set; }
    [field: SerializeField] public float StoppingDistance { get; private set; }
    [field: SerializeField] public float DestroyBeforeDeathDelay { get; private set; }
  }
}