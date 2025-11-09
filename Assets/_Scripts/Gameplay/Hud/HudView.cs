using _Scripts.Gameplay.Hud.Enemies;
using _Scripts.Gameplay.Hud.Ping;
using _Scripts.Infrastructure.Scopes.NetCore;
using UnityEngine;
using VContainer;

namespace _Scripts.Gameplay.Hud
{
  public class HudView : MonoBehaviour
  {
    [SerializeField] private PingPresenter _pingPresenter;
    [SerializeField] private EnemiesCountPresenter _enemiesCountPresenter;
    
    [Inject] private readonly IPingModel _pingModel;
    [Inject] private readonly IReadOnlyNetworkRoomModel _roomModel;
    
    private void Awake()
    {
      _pingPresenter.Construct(_pingModel);
      _pingPresenter.Initialize();
      _enemiesCountPresenter.Construct(_roomModel);
      _enemiesCountPresenter.Initialize();
    }
  }
}