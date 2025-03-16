using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.health.UI
{
  public class HealthView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetHealth(float value) => 
      _healthText.text = $"Health: {value}";
  }
}