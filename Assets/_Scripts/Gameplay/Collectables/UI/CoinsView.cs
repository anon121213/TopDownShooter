using TMPro;
using UnityEngine;

namespace _Scripts.Gameplay.Collectables.UI
{
  public class CoinsView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void ChangeCoinsValue(int value) =>
      _coinsText.text = $"Coins: {value}";
  }
}