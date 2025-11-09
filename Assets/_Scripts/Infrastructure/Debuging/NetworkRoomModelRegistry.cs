using _Scripts.Infrastructure.Scopes.NetCore;

namespace _Scripts.Infrastructure.Debuging
{
  public static class NetworkRoomModelRegistry
  {
    public static NetworkRoomModel Model;

    public static void Register(NetworkRoomModel model)
    {
      Model = model;
    }

    public static void Unregister(NetworkRoomModel model)
    {
      if (Model == model)
        Model = null;
    }
  }
}