using _Scripts.Gameplay.PlayerCamera.Data;
using _Scripts.Gameplay.PlayerCamera.Services;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Warmup;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Gameplay.PlayerCamera.Factory
{
  public class CameraFactory : ICameraFactory
  {
    private readonly ICameraProvider _cameraProvider;
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IAssetProvider _assetProvider;
    private readonly ICameraFollower _cameraFollower;
    private readonly ICameraController _cameraController;
    private CameraConfig _config;

    public CameraFactory(ICameraProvider cameraProvider,
      IStaticDataProvider staticDataProvider,
      IAssetProvider assetProvider,
      ICameraFollower cameraFollower,
      ICameraController cameraController)
    {
      _cameraProvider = cameraProvider;
      _staticDataProvider = staticDataProvider;
      _assetProvider = assetProvider;
      _cameraFollower = cameraFollower;
      _cameraController = cameraController;
    }

    public async UniTask Warmup()
    {
      _config = _staticDataProvider.GetConfig<CameraConfig>();
      await _assetProvider.LoadAssetAsync(_config.Prefab);
    }

    public async UniTask CreateCamera(Transform follow)
    {
      Camera prefab = await _assetProvider.LoadAssetAsync<Camera>(_config.Prefab);
      Camera camera = Object.Instantiate(prefab, _config.Position, Quaternion.Euler(_config.Rotation));
      
      _cameraProvider.SetCamera(camera);
      _cameraFollower.SetTarget(follow);
      _cameraController.EnableServices();
    }
  }

  public interface ICameraFactory : IWarmupable
  {
    UniTask CreateCamera(Transform follow);
  }
}