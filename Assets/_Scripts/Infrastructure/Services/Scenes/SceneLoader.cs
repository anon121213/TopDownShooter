using System;
using FishNet.Managing;
using FishNet.Managing.Scened;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace _Scripts.Infrastructure.Services.Scenes
{
  public class SceneLoader : ISceneLoader
  {
    private readonly NetworkManager _networkManager;

    public SceneLoader(NetworkManager networkManager)
    {
      _networkManager = networkManager;
    }

    public void Load(string sceneName, Action onLoaded = null)
      => LoadScene(sceneName, onLoaded);

    private void LoadScene(string sceneName, Action onLoaded = null)
    {
      var currentSceneName = SceneManager.GetActiveScene().name;
      _networkManager.SceneManager.OnLoadEnd += Handler;
      _networkManager.SceneManager.LoadConnectionScenes(new SceneLoadData(sceneName));
      return;

      void Handler(SceneLoadEndEventArgs args)
      {
        _networkManager.SceneManager.OnLoadEnd -= Handler;
        onLoaded?.Invoke();
        Unload(currentSceneName);
      }
    }

    private void Unload(string sceneName) => 
      _networkManager.SceneManager.UnloadConnectionScenes(new SceneUnloadData(sceneName));
  }
}