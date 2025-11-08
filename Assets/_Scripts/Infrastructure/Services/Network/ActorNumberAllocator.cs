using System.Collections.Generic;
using _Scripts.Infrastructure.Scopes.NetCore;

namespace _Scripts.Infrastructure.Services.Network
{
  public class ActorNumberAllocator : IActorNumberAllocator
  {
    private readonly IReadOnlyNetworkRoomModel _roomModel;

    public ActorNumberAllocator(IReadOnlyNetworkRoomModel roomModel) => 
      _roomModel = roomModel;

    public int GetMobActorNumber()
    {
      var used = new HashSet<int>();
      foreach (var mob in _roomModel.MobsDto) 
        used.Add(mob.Value.ActorNumber);

      int number = NetworkConstants.MOB_ACTOR_NUMBERS_OFFSET;
      while (used.Contains(number))
        number++;

      return number;
    }
  }

  public interface IActorNumberAllocator
  {
    int GetMobActorNumber();
  }
}