using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Hud.Ping
{
  public class PingPresenter : MonoBehaviour, IInitializable
  {
    [SerializeField] private PingView _pingView;
    
    private IPingModel _pingModel;

    public void Construct(IPingModel pingModel) => 
      _pingModel = pingModel;

    public void Initialize() => 
      _pingModel.Ping.StartWith(_pingModel.Ping.Value)
        .Subscribe(ping => _pingView.SetPing(ping))
        .AddTo(this);
  }
}