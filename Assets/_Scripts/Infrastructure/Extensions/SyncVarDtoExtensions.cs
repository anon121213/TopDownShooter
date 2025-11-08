using FishNet.Object.Synchronizing;

namespace _Scripts.Infrastructure.Extensions
{
  public static class SyncVarDtoExtensions
  {
    public static void With<T>(this SyncVar<T> syncVar, System.Action<T> mutate)
      where T : struct
    {
      var value = syncVar.Value; 
      mutate(value);             
      syncVar.Value = value;    
    }
  }
}