using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Gameplay.PlayerCamera.Data
{
  [CreateAssetMenu(menuName = "Data/Configs/Camera/CameraConfig", fileName = "CameraConfig")]
  public class CameraConfig : ScriptableObject
  {
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public Vector3 Rotation { get; private set; }
    [field: SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public float FollowSpeed { get; private set; }
  }
}