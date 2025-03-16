using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Scripts.Infrastructure.Services.Scenes
{
  public class SceneLoader : ISceneLoader
  {
    public async UniTask Load(string sceneName, Action onLoaded = null) =>
      await LoadScene(sceneName, onLoaded);

    private async UniTask LoadScene(string sceneName, Action onLoaded = null)
    {
      await Addressables.LoadSceneAsync(sceneName);
      onLoaded?.Invoke();
    }
  }
}