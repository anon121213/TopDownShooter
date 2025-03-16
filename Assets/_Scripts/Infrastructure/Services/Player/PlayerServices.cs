using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace _Scripts.Infrastructure.Services.Player
{
  public class PlayerServices : IPlayerServices, IDisposable, ITickable
  {
    private readonly List<IPlayerService> _services = new();
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IInitializable> _initializables = new();

    public void AddService(IPlayerService service)
    {
      if (_services.Contains(service))
        return;

      _services.Add(service);
      
      if (service is IUpdatable updatable) 
        _updatables.Add(updatable);

      if (service is IInitializable initializable) 
        _initializables.Add(initializable);
    }

    public void Tick()
    {
      foreach (var updatable in _updatables)
        updatable.OnUpdate();
    }

    public void InitializeServices()
    {
      foreach (var service in _initializables)
        service.Initialize();
    }
    
    public void EnableServices()
    {
      foreach (var service in _services)
        service.Enable();
    }

    public void DisableServices()
    {
      foreach (var service in _services)
        service.Disable();
    }

    public void Dispose() =>
      DisableServices();
  }

  public interface IUpdatable
  {
    void OnUpdate();
  }

  public interface IInitializable
  {
    void Initialize();
  }

  public interface IPlayerService
  {
    void Enable();
    void Disable();
  }

  public interface IPlayerServices
  {
    void AddService(IPlayerService service);
    void InitializeServices();
    void EnableServices();
    void DisableServices();
  }
}