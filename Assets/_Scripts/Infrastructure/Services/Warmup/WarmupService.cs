using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Warmup
{
  public class WarmupService : IWarmupService
  {
    private readonly IEnumerable<IWarmupable> _warmupables;
    
    public WarmupService(IEnumerable<IWarmupable> warmupables) => 
      _warmupables = warmupables;

    public async UniTask Warmup(CancellationToken ct)
    {
      foreach (var warmupable in _warmupables) 
        await warmupable.Warmup(ct);
    }
  }

  public interface IWarmupable
  {
    UniTask Warmup(CancellationToken ct);
  }
}