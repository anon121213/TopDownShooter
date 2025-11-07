using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infrastructure.Services.Data.DataProvider
{
  public interface IStaticDataProvider
  {
    UniTask Initialize(CancellationToken ct);
    TData GetConfig<TData>();
  }
}