using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Player;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  [CreateAssetMenu(menuName = "Data/Configs/Network/NetworkConfig", fileName = "NetworkConfig", order = 0)]
  public class NetworkConfig : ScriptableObject
  {
    [field: SerializeField] public ConnectType ConnectType { get; private set; }
    [field: SerializeField] public PlayerScope PlayerScopePrefab { get; private set; }
    [field: SerializeField] public MobScope MobScopePrefab { get; private set; }
  }

  public enum ConnectType
  {
    Host = 0,
    Client = 1
  }
}