using _Scripts.Infrastructure.Services.Warmup;

namespace _Scripts.Infrastructure.Services.Data.DataProvider
{
  public interface IStaticDataProvider : IWarmupable
  {
    TData GetConfig<TData>();
  }
}