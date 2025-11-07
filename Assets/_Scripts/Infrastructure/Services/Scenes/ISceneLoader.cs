using System;

namespace _Scripts.Infrastructure.Services.Scenes
{
  public interface ISceneLoader
  {
    void Load(string sceneName, Action onLoaded = null);
  }
}