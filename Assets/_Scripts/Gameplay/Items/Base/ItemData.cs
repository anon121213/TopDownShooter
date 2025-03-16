using System;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Items.Base
{
  [Serializable]
  public class ItemData
  {
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public float ReloadDelay { get; private set; }
    public readonly ReactiveProperty<int> Count = new(0);
  }
}