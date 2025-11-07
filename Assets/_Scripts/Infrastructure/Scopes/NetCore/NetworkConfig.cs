using UnityEngine;

namespace _Scripts.Infrastructure.NetCore
{
  [CreateAssetMenu(menuName = "Data/Configs/Network/NetworkConfig", fileName = "NetworkConfig", order = 0)]
  public class NetworkConfig : ScriptableObject
  {
    [field: SerializeField] public ConnectType ConnectType { get; private set; }
  }

  public enum ConnectType
  {
    Host = 0,
    Client = 1
  }
}