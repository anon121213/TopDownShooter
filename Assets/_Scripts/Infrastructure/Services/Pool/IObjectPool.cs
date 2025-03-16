using UnityEngine;

namespace _Scripts.Infrastructure.Services.Pool
{
    public interface IObjectPool
    {
        T GetGameObject<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour;
        T GetGameObject<T>(T prefab, Vector3 position, Quaternion rotation, Transform root) where T : MonoBehaviour;
        GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation);
        void ReturnGameObject<T>(GameObject tGameObject, T prefab) where T : MonoBehaviour;
        void ReturnGameObject(GameObject tGameObject, GameObject mPrefab);
    }
}