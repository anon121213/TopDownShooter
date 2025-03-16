using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Warmup
{
  public interface IWarmupService
  {
    UniTask Warmup();
  }
}