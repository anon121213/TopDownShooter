using System.Threading;
using _Scripts.Infrastructure.Constants;
using _Scripts.Infrastructure.Services.Scenes;
using _Scripts.Infrastructure.Services.Warmup;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Scopes.Game
{
  public class GameService : IAsyncStartable
  {
    private readonly ISceneLoader _sceneLoader;
    private readonly IWarmupService _warmupService;

    public GameService(ISceneLoader sceneLoader,
      IWarmupService warmupService)
    {
      _sceneLoader = sceneLoader;
      _warmupService = warmupService;
    }

    public async Awaitable StartAsync(CancellationToken cancellation = new())
    {
      await _warmupService.Warmup(cancellation);
      _sceneLoader.Load(GameConstants.GAME_SCENE);
    }
  }
}