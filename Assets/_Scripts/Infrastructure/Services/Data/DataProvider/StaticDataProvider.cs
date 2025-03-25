using System;
using System.Collections.Generic;
using _Scripts.Infrastructure.Services.Data.AssetLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Infrastructure.Services.Data.DataProvider
{
  public class StaticDataProvider : IStaticDataProvider
  {
    private const string Config = "Config";
    
    private readonly IAssetProvider _assetProvider;
    private List<ScriptableObject> _staticData;

    public StaticDataProvider(IAssetProvider assetProvider) => 
      _assetProvider = assetProvider;

    public async UniTask Warmup()
    {
      _staticData = await _assetProvider.LoadAssetsByLabelAsync<ScriptableObject>(Config);
    }

    public TData GetConfig<TData>() => 
      GetFirstDataOfType<TData>();

    private TData GetFirstDataOfType<TData>()
    {
      TData firstData = default(TData);

      foreach (ScriptableObject data in _staticData)
      {
        if (data is TData dataOfType)
        {
          firstData = dataOfType;
          break;
        }
      }

      return firstData;
    }

    private List<TData> GetListDataOfType<TData>()
    {
      List<TData> listData = new List<TData>();

      foreach (ScriptableObject data in _staticData)
      {
        if (data is TData dataOfType)
          listData.Add(dataOfType);
      }

      return listData;
    }
  }
}