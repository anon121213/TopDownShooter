using _Scripts.Infrastructure.Services.Scenes;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Bootstrapper
{
  public class MainBootstrapper : IInitializable
  {
    private const int FRAMERATE = 240;

    private readonly ISceneLoader _sceneLoader;

    public MainBootstrapper(ISceneLoader sceneLoader) => 
      _sceneLoader = sceneLoader;

    public async void Initialize()
    {
      Application.targetFrameRate = FRAMERATE;
      await _sceneLoader.Load(SceneNamesConstants.GameScene);
    }
  }
}