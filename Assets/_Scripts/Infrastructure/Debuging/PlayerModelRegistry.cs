using System.Collections.Generic;
using _Scripts.Gameplay.Player;

namespace _Scripts.Infrastructure.Debuging
{
  public static class PlayerModelRegistry
  {
    public static readonly HashSet<PlayerModel> Models = new();

    public static void Register(PlayerModel model)
    {
      Models.Add(model);
    }

    public static void Unregister(PlayerModel model)
    {
      Models.Remove(model);
    }
  }
}