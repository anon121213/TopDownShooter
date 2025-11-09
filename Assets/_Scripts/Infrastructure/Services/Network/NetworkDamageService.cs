using _Scripts.Infrastructure.Scopes.NetCore;
using UnityEngine;
using VContainer;

namespace _Scripts.Infrastructure.Services.Network
{
  public class NetworkDamageService : INetworkDamageService
  {
    [Inject] private readonly INetworkRoomModel _roomModel;
    
    public void SendDamageToPlayer(DamageData data)
    {
      if (_roomModel.IsServer.Value == false)
      {
        Debug.LogError("Send damage can only server");
        return;
      }
      
      ApplyDamageToPlayer(data);
    }

    public void SendDamageToEnemy(DamageData data)
    {
      if (_roomModel.IsServer.Value == false)
      {
        Debug.LogError("Send damage can only server");
        return;
      }
      
      ApplyDamageToMob(data);
    }

    private void ApplyDamageToPlayer(DamageData data)
    {
      foreach (var player in _roomModel.PlayersRoot)
      {
        if (player.Value.ActorNumber.Value != data.ActorId)
          continue;
        
        player.Value.SetHealth(player.Value.Health.Value - data.Damage);
      }
    }

    private void ApplyDamageToMob(DamageData data)
    {
      foreach (var mob in _roomModel.MobsRoot)
      {
        if (mob.Value.ActorNumber.Value != data.ActorId)
          continue;
        
        mob.Value.SetHealth(mob.Value.Health.Value - data.Damage);
      }
    }
  }

  public interface INetworkDamageService
  {
    void SendDamageToPlayer(DamageData data);
    void SendDamageToEnemy(DamageData data);
  }

  public struct DamageData
  {
    public readonly int ActorId;
    public readonly float Damage;

    public DamageData(int actorId, float damage)
    {
      Damage = damage;
      ActorId = actorId;
    }
  }
}