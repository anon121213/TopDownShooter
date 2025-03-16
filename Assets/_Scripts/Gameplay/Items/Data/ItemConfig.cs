using _Scripts.Gameplay.Items.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Data
{
  public abstract class ItemConfig : ScriptableObject
  {
    [field: SerializeField] public ItemData ItemData { get; private set; }
  }
}