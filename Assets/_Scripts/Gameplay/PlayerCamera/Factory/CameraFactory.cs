using System;
using System.Threading;
using _Scripts.Gameplay.PlayerCamera.Data;
using _Scripts.Gameplay.PlayerCamera.Services;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Gameplay.PlayerCamera.Factory
{
  public class CameraFactory : ICameraFactory, IDisposable
  {
    private readonly ICameraProvider _cameraProvider;
    private readonly ICameraFollower _cameraFollower;
    private readonly ICameraController _cameraController;
    private readonly CameraConfig _config;

    private readonly CancellationTokenSource _cts = new();

    public CameraFactory(ICameraProvider cameraProvider,
      IStaticDataProvider staticDataProvider,
      ICameraFollower cameraFollower,
      ICameraController cameraController)
    {
      _cameraProvider = cameraProvider;
      _cameraFollower = cameraFollower;
      _cameraController = cameraController;
      _config = staticDataProvider.GetConfig<CameraConfig>();
    }
    
    public void CreateCamera(Transform follow)
    {
      Camera camera = Object.Instantiate(_config.Prefab, _config.Position, Quaternion.Euler(_config.Rotation));
      
      _cameraProvider.SetCamera(camera);
      _cameraFollower.SetTarget(follow);
      _cameraController.EnableServices();
    }

    public void Dispose() => 
      _cts?.Dispose();
  }

  public interface ICameraFactory
  {
    void CreateCamera(Transform follow);
  }
}