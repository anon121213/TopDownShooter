using System;
using FishNet.Managing;
using UniRx;
using VContainer.Unity;

namespace _Scripts.Gameplay.Hud.Ping
{
  public class PingModel : IPingModel, IInitializable, IDisposable
  {
    private readonly NetworkManager _networkManager;
    private readonly ReactiveProperty<int> _ping = new();
    public IReadOnlyReactiveProperty<int> Ping => _ping;

    private IDisposable _disposable;
    
    public PingModel(NetworkManager networkManager) => 
      _networkManager = networkManager;

    public void Initialize()
    {
      if (_networkManager.IsServerStarted)
      {
        _ping.Value = 0;
        return;
      }

      _disposable = Observable.Interval(TimeSpan.FromSeconds(1))
        .Subscribe(_ => _ping.Value = (int)_networkManager.TimeManager.RoundTripTime);
    }

    public void Dispose() => 
      _disposable?.Dispose();
  }

  public interface IPingModel
  {
    IReadOnlyReactiveProperty<int> Ping { get; }
  }
}