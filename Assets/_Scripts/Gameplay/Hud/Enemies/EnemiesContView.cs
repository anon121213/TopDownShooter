using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Hud.Enemies
{
  public class EnemiesContView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _countTextField;
    [SerializeField] private string _countText;

    public void SetCount(int count) => 
      _countTextField.text = string.Format(_countText, count);
  }
}