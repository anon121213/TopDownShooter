using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _Scripts.Gameplay.Player.Services.Base
{
  public class PlayerServices : IPlayerServices, IDisposable, ITickable
  {
    private readonly List<PlayerService> _services = new();

    public PlayerServices(IEnumerable<PlayerService> playerServices)
    {
      foreach (var playerService in playerServices) 
        AddService(playerService);
    }

    private void AddService(PlayerService service)
    {
      if (_services.Contains(service))
        return;

      _services.Add(service);
    }

    public void Tick()
    {
      foreach (var service in _services)
        service.OnUpdate();
    }

    public void ConstructServices(PlayerRootView playerRootView)
    {
      foreach (var service in _services) 
        service.Construct(playerRootView);
    }
    
    public void InitializeServices()
    {
      foreach (var service in _services) 
        service.OnInitialize();
    }
    
    public void EnableServices()
    {
      foreach (var service in _services)
        service.OnEnable();
    }

    public void DisableServices()
    {
      foreach (var service in _services)
        service.OnDisable();
    }

    public void Dispose()
    {
      foreach (var service in _services)
        service.OnDispose();
    }
  }
  public abstract class PlayerService
  {
    protected PlayerRootView PlayerRoot { get; private set; }
    protected bool IsEnable { get; private set; }
    protected CompositeDisposable Disposables { get; private set; } = new();
    
    public void Construct(PlayerRootView playerRootView){ PlayerRoot = playerRootView; }

    public virtual void OnInitialize() { }
    public virtual void OnUpdate() { }

    public virtual void OnEnable() { IsEnable = true; }
    public virtual void OnDisable() { IsEnable = false; }

    public virtual void OnDispose() { Disposables.Dispose(); OnDisable(); }
  }

  public interface IPlayerServices
  {
    void ConstructServices(PlayerRootView playerRoot);
    void InitializeServices();
    void EnableServices();
    void DisableServices();
  }
}