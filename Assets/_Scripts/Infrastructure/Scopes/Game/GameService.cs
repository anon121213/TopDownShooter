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
    private readonly GameScope _gameScope;
    private readonly ISceneLoader _sceneLoader;
    private readonly IWarmupService _warmupService;

    public GameService(GameScope gameScope, 
      ISceneLoader sceneLoader,
      IWarmupService warmupService)
    {
      _gameScope = gameScope;
      _sceneLoader = sceneLoader;
      _warmupService = warmupService;
    }

    public async Awaitable StartAsync(CancellationToken cancellation = new())
    {
      await _warmupService.Warmup(cancellation);
      LifetimeScope.EnqueueParent(_gameScope);
      _sceneLoader.Load(GameConstants.GAME_SCENE);
    }
  }
}