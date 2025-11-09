using UnityEngine;

namespace _Scripts.Gameplay.Hud
{
  [CreateAssetMenu(menuName = "Data/Configs/Hud/HudConfig", fileName = "HudConfig")]
  public class HudConfig : ScriptableObject
  {
    [field: SerializeField] public HudView HudPrefab { get; private set; }
  }
}