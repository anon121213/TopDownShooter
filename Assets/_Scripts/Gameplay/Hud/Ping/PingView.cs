using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Hud.Ping
{
  public class PingView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private string _textFormat;

    public void SetPing(int ping) => 
      _textField.text = string.Format(_textFormat, ping);
  }
}