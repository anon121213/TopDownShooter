using _Scripts.Infrastructure.Scopes.NetCore;
using UniRx;
using UnityEngine;

namespace _Scripts.Gameplay.Hud.Enemies
{
  public class EnemiesCountPresenter : MonoBehaviour
  {
    [SerializeField] private EnemiesContView _enemiesContView;
    
    private IReadOnlyNetworkRoomModel _roomModel;

    public void Construct(IReadOnlyNetworkRoomModel roomModel) => 
      _roomModel = roomModel;

    public void Initialize()
    {
      _roomModel.MobsDto
        .ObserveCountChanged()
        .StartWith(_roomModel.MobsDto.Count)
        .Subscribe(count => _enemiesContView.SetCount(count))
        .AddTo(this);
    }
  }
}