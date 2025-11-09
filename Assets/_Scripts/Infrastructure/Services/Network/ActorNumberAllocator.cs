using System.Collections.Generic;
using _Scripts.Infrastructure.Scopes.NetCore;

namespace _Scripts.Infrastructure.Services.Network
{
  public static class ActorNumberAllocator
  {
    public static int GetMobActorNumber(IReadOnlyNetworkRoomModel roomModel)
    {
      var used = new HashSet<int>();
      foreach (var mob in roomModel.MobsDto) 
        used.Add(mob.Value.ActorNumber);

      int number = NetworkConstants.MOB_ACTOR_NUMBERS_OFFSET;
      while (used.Contains(number))
        number++;

      return number;
    }
    
    public static int GetProjectileActorNumber(IReadOnlyNetworkRoomModel roomModel)
    {
      var used = new HashSet<int>();
      foreach (var projectile in roomModel.ProjectilesDto) 
        used.Add(projectile.Value.ActorNumber);

      int number = NetworkConstants.PROJECTILE_ACTOR_NUMBERS_OFFSET;
      while (used.Contains(number))
        number++;

      return number;
    }
  }
}