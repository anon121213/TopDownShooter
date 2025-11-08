using System.Collections.Generic;
using _Scripts.Gameplay.Enemies;

namespace _Scripts.Infrastructure.Debuging
{
  public static class MobModelRegistry
  {
    public static readonly HashSet<MobModel> Models = new();

    public static void Register(MobModel model)
    {
      Models.Add(model);
    }

    public static void Unregister(MobModel model)
    {
      Models.Remove(model);
    }
  }
}