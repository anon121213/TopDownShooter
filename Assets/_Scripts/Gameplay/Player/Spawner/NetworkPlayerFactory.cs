using System;
using _Scripts.Gameplay.Player.Data;
using _Scripts.Infrastructure.Scopes.NetCore;
using _Scripts.Infrastructure.Services.Data.DataProvider;
using FishNet.Managing;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace _Scripts.Gameplay.Player.Spawner
{
  public class NetworkPlayerFactory : IInitializable, INetworkPlayerFactory
  {
    private readonly IStaticDataProvider _staticDataProvider;
    private readonly IReadOnlyNetworkRoomModel _roomModel;
    private readonly NetworkManager _networkManager;
    
    private PlayerConfig _playerConfig;

    public NetworkPlayerFactory(IStaticDataProvider staticDataProvider,
      IReadOnlyNetworkRoomModel roomModel,
      NetworkManager networkManager)
    {
      _staticDataProvider = staticDataProvider;
      _roomModel = roomModel;
      _networkManager = networkManager;
    }

    public void Initialize() => 
      _playerConfig = _staticDataProvider.GetConfig<PlayerConfig>();

    public NetworkPlayerView CreateNetworkPlayer(Vector3 position, Quaternion rotation, PlayerModelDTO playerModelDto)
    {
      if (!_roomModel.IsServer.Value)
      {
        Debug.LogError("NetworkPlayer can spawn only server!!!");
        return null;
      }
      
      var player = Object.Instantiate(_playerConfig.NetworkPlayerPrefab, position, rotation);
      _networkManager.ServerManager.Spawn(player.gameObject, _networkManager.ServerManager.Clients[playerModelDto.ActorNumber]);
      player.PlayerModel.Apply(playerModelDto);
      return player;
    }
  }

  public interface INetworkPlayerFactory
  {
    NetworkPlayerView CreateNetworkPlayer(Vector3 position, Quaternion rotation, PlayerModelDTO playerModelDto);
  }
}