using _Scripts.Gameplay.health.Data;
using _Scripts.Gameplay.health.UI;
using _Scripts.Infrastructure.Services.Player;

namespace _Scripts.Gameplay.health
{
  public class PlayerHealth : IInitializable, IPlayerHealth
  {
    private Health _health;
    private IHealthPresenter _healthPresenter;
    private PlayerHealthConfig _config;

    public void Construct(Health health,
      IHealthPresenter healthPresenter,
      PlayerHealthConfig config)
    {
      _health = health;
      _healthPresenter = healthPresenter;
      _config = config;
    }

    public void Initialize()
    {
      _health.Construct(_config.InitHealth);
      _healthPresenter.Initialize(_health);
    }

    public void Enable() { }
    public void Disable() { }
  }
  
  public interface IPlayerHealth : IPlayerService
  {
    void Construct(Health health,
      IHealthPresenter healthPresenter,
      PlayerHealthConfig config);
  }
}

