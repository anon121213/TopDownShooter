using _Scripts.Gameplay.Enemies;
using _Scripts.Gameplay.Player;
using UnityEngine;

namespace _Scripts.Infrastructure.Scopes.NetCore
{
  [CreateAssetMenu(menuName = "Data/Configs/Network/NetworkConfig", fileName = "NetworkConfig", order = 0)]
  public class NetworkConfig : ScriptableObject
  {
    [field: SerializeField] public ConnectType ConnectType { get; private set; }
    [field: SerializeField] public LocalPlayerScope LocalPlayerScopePrefab { get; private set; }
    [field: SerializeField] public RemotePlayerScope RemotePlayerScopePrefab { get; private set; }
    [field: SerializeField] public LocalMobScope LocalMobScopePrefab { get; private set; }

    [field: SerializeField] public float NetworkSendDelayMS { get; private set; }
  }

  public enum ConnectType
  {
    Host = 0,
    Client = 1
  }
}