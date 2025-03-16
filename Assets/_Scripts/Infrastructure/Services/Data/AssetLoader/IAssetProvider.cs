using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Scripts.Infrastructure.Services.Data.AssetLoader
{
  public interface IAssetProvider
  {
    UniTask<GameObject> LoadAssetAsync(AssetReference path);
    UniTask<TObject> LoadAssetAsync<TObject>(AssetReference path) where TObject : Component;
    UniTask<List<T>> LoadAssetsByLabelAsync<T>(string label) where T : class;
    void Cleanup();
  }
}