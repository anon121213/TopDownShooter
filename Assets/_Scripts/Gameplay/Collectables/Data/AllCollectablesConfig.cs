using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Collectables/AllCollectablesConfig", fileName = "AllCollectablesConfig", order = 0)]
  public class AllCollectablesConfig : ScriptableObject
  {
    [SerializeField] private List<CollectableConfig> _collectablesConfigs;

    public IReadOnlyList<CollectableConfig> Configs => _collectablesConfigs;
    
    public CollectableConfig GetConfig(CollectableType type) => 
      _collectablesConfigs.FirstOrDefault(x => x.Type == type);
  }

  public enum CollectableType
  {
    Unknown = 0,
    Coin = 1
  }
}