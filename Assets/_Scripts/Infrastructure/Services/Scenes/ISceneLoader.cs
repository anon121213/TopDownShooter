using System;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Scenes
{
  public interface ISceneLoader
  {
    UniTask Load(string sceneName, Action onLoaded = null);
  }
}