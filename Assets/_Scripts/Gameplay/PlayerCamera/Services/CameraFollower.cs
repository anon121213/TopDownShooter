using _Scripts.Gameplay.PlayerCamera.Data;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using UnityEngine;

namespace _Scripts.Gameplay.PlayerCamera.Services
{
  public class CameraFollower : ICameraFollower
  {
    private readonly IStaticDataProvider _staticDataProvider;
    
    private Transform _target;
    private float _followSpeed;
    private Vector3 _offset;

    public CameraFollower(IStaticDataProvider staticDataProvider) =>
      _staticDataProvider = staticDataProvider;

    public void SetTarget(Transform target)
    {
      _followSpeed = _staticDataProvider.GetConfig<CameraConfig>().FollowSpeed;
      _offset = _staticDataProvider.GetConfig<CameraConfig>().Offset;
      _target = target;
    }

    public void Move()
    {
      if (!_target)
        return;

      Vector3 targetPosition = _target.position + _offset;
      CameraProvider.MainCamera.transform.position = Vector3.Lerp(CameraProvider.MainCamera
        .transform.position, targetPosition, _followSpeed * Time.deltaTime);
    }
  }

  public interface ICameraFollower
  {
    void SetTarget(Transform target);
    void Move();
  }
}