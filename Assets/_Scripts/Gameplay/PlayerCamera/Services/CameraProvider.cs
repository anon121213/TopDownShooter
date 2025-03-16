using UnityEngine;

namespace _Scripts.Gameplay.PlayerCamera.Services
{
  public class CameraProvider : ICameraProvider
  {
    public static Camera MainCamera { get; private set; }

    public void SetCamera(Camera camera) => 
      MainCamera = camera;
  }

  public interface ICameraProvider
  {
    void SetCamera(Camera camera);
  }
}