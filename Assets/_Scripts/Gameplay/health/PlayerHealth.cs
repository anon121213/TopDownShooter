using _Scripts.Gameplay.health.Data;
using _Scripts.Gameplay.health.UI;
using _Scripts.Gameplay.Player;
using _Scripts.Gameplay.Player.Services.Base;

namespace _Scripts.Gameplay.health
{
  public class PlayerHealth : IInitializable, IPlayerHealth
  {
    private readonly IReadOnlyPlayerModel _playerModel;
    private readonly Health _health;
    
    private IHealthPresenter _healthPresenter;
    private PlayerHealthConfig _config;

    public PlayerHealth(PlayerView playerView, IReadOnlyPlayerModel playerModel)
    {
      _playerModel = playerModel;
      _health = playerView.Health;
    }

    public void Initialize()
    {
      _config = _playerModel.PlayerConfig.HealthConfig;
      _health.Construct(_config.InitHealth);

      //_healthPresenter.Initialize(_health);
    }

    public void Enable() { }
    public void Disable() { }
  }
  
  public interface IPlayerHealth : IPlayerService
  {
  }
}

