using System;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.Base
{
  public abstract class Collectable : MonoBehaviour
  {
    public abstract int Points { get; protected set; }
    public abstract void Claim();
    public abstract void Construct(int points);
    public abstract event Action<Collectable> OnCollect;
  }
}