using System;
using _Scripts.Infrastructure.Scopes.NetCore;
using FishNet.Managing;
using FishNet.Managing.Scened;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace _Scripts.Infrastructure.Services.Scenes
{
  public class SceneLoader : ISceneLoader
  {
    private readonly NetworkManager _networkManager;
    private readonly IReadOnlyNetworkRoomModel _roomModel;

    public SceneLoader(NetworkManager networkManager,
      IReadOnlyNetworkRoomModel roomModel)
    {
      _networkManager = networkManager;
      _roomModel = roomModel;
    }

    public void Load(string sceneName, Action onLoaded = null)
      => LoadScene(sceneName, onLoaded);

    private void LoadScene(string sceneName, Action onLoaded = null)
    {
      if(!_roomModel.IsServer.Value)
        return;
      
      var currentSceneName = SceneManager.GetActiveScene().name;
      _networkManager.SceneManager.OnLoadEnd += Handler;
      _networkManager.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneName));
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