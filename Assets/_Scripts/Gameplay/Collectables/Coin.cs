using System;
using _Scripts.Gameplay.Collectables.Base;

namespace _Scripts.Gameplay.Collectables
{
  public class Coin : Collectable
  {
    public override int Points { get; protected set; }
    public override event Action<Collectable> OnCollect;
    
    public override void Construct(int points) => 
      Points = points;

    public override void Claim() => 
      OnCollect?.Invoke(this);
  }
}