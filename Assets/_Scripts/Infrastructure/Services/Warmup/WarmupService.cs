using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Warmup
{
  public class WarmupService : IWarmupService
  {
    private readonly IEnumerable<IWarmupable> _warmupables;

    public WarmupService(IEnumerable<IWarmupable> warmupables) => 
      _warmupables = warmupables;

    public async UniTask Warmup()
    {
      foreach (var warmupable in _warmupables) 
        await warmupable.Warmup();
    }
  }

  public interface IWarmupable
  {
    UniTask Warmup();
  }
}