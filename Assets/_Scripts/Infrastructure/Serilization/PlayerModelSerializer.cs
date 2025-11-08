using _Scripts.Gameplay.Player;
using FishNet.Serializing;

namespace _Scripts.Infrastructure.Serilization
{
  public static class PlayerStateSerializer
  {
    public static void WritePlayerStateDTO(this Writer w, PlayerStateDTO dto)
    {
      w.WriteInt32(dto.ActorNumber);
      w.WriteBoolean(dto.IsLocal);
      w.WriteBoolean(dto.IsDead);
      w.WriteSingle(dto.Health);
    }

    public static PlayerStateDTO ReadPlayerStateDTO(this Reader r) => 
      new(r.ReadInt32(), r.ReadBoolean(), r.ReadBoolean(), r.ReadSingle());
  }
}