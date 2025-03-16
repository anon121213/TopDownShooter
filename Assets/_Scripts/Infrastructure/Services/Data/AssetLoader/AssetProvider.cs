using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts.Infrastructure.Services.Data.AssetLoader
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, List<AsyncOperationHandle>> _usedResources = new();

    public async UniTask<GameObject> LoadAssetAsync(AssetReference path)
    {
      var handle = Addressables.LoadAssetAsync<GameObject>(path);
      await handle.Task;

      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        RegisterForCleanup(path.RuntimeKey.ToString(), handle);
        return handle.Result;
      }

      Debug.LogError($"Не удалось загрузить префаб по пути: {path}");
      return null;
    }

    public async UniTask<TObject> LoadAssetAsync<TObject>(AssetReference path) where TObject : Component
    {
      var handle = Addressables.LoadAssetAsync<GameObject>(path);
      await handle.Task;

      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        handle.Result.TryGetComponent(out TObject component);
        if (component != null)
        {
          RegisterForCleanup(path.RuntimeKey.ToString(), handle);
          return component;
        }
      }

      Debug.LogError($"Не удалось загрузить компонент {typeof(TObject).Name} из префаба по пути: {path}");
      return null;
    }

    public async UniTask<List<T>> LoadAssetsByLabelAsync<T>(string label) where T : class
    {
      var handle = Addressables.LoadAssetsAsync<T>(label);
      await handle.Task;

      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        RegisterForCleanup(label, handle);
        return handle.Result.ToList();
      }

      Debug.LogError($"Не удалось загрузить ассеты по метке: {label}");
      return new List<T>();
    }

    public void Cleanup()
    {
      foreach (var key in _usedResources.Keys.ToList())
      {
        for (int i = _usedResources[key].Count - 1; i >= 0; i--)
        {
          var handle = _usedResources[key][i];

          if (handle.IsValid())
            Addressables.Release(handle);

          _usedResources[key].RemoveAt(i);
        }

        _usedResources.Remove(key);
      }
    }

    private void RegisterForCleanup<T>(string key, AsyncOperationHandle<T> handle)
    {
      if (!handle.IsValid())
        return;

      if (!_usedResources.TryGetValue(key, out var resourceHandles))
      {
        resourceHandles = new List<AsyncOperationHandle>();
        _usedResources[key] = resourceHandles;
      }

      resourceHandles.Add(handle);
    }
  }
}