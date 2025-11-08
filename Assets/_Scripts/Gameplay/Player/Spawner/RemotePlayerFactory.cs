using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using _Scripts.Infrastructure.Services.Pool;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player.Spawner
{
  public class RemotePlayerFactory : IInitializable, IRemotePlayerFactory
  {
    private readonly IObjectPool _objectPool;
    private readonly IStaticDataProvider _staticDataProvider;
    private PlayerConfig _playerConfig;
    
    public RemotePlayerFactory(IStaticDataProvider staticDataProvider) => 
      _staticDataProvider = staticDataProvider;

    public void Initialize() => 
      _playerConfig = _staticDataProvider.GetConfig<PlayerConfig>();
    
    public RemotePlayerView CreateRemotePlayer(Vector3 position, Quaternion rotation, Transform root)
    {
      var player = Object.Instantiate(_playerConfig.RemotePlayerPrefab, root);
      player.transform.localPosition = position;
      player.transform.localRotation = rotation;
      return player;
    }
  }

  public interface IRemotePlayerFactory
  {
    RemotePlayerView CreateRemotePlayer(Vector3 position, Quaternion rotation, Transform root);
  }
}