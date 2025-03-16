using System;
using UniRx;

namespace _Scripts.Gameplay.PlayerCamera.Services
{
  public class CameraController : ICameraController, IDisposable
  {
    private readonly ICameraFollower _cameraFollower;
    private IDisposable _disposable;

    public CameraController(ICameraFollower cameraFollower) => 
      _cameraFollower = cameraFollower;

    public void EnableServices() => 
      SubscribeServices();

    public void DisableServices() => 
      _disposable?.Dispose();

    private void SubscribeServices() => 
      _disposable = Observable.EveryUpdate()
        .Subscribe(_ => _cameraFollower.Move());

    public void Dispose() => 
      _disposable.Dispose();
  }

  public interface ICameraController
  {
    void EnableServices();
    void DisableServices();
  }
}