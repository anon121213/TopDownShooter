using System.Linq;
using _Scripts.Gameplay.Items.Weapons;
using _Scripts.Infrastructure.Services.Input;
using _Scripts.Infrastructure.Services.Player;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerInventory : IPlayerService
  {
    private readonly IInputService _inputService;
    private readonly IPlayerBackpack _playerBackpack;
    private readonly IPlayerAttacker _playerAttacker;

    private int _currentWeaponIndex;

    public PlayerInventory(IInputService inputService,
      IPlayerBackpack playerBackpack,
      IPlayerAttacker playerAttacker)
    {
      _inputService = inputService;
      _playerBackpack = playerBackpack;
      _playerAttacker = playerAttacker;
    }

    public void Enable()
    {
      _inputService.OnChangeWeapon += TrySetWeapon;
    }

    private void TrySetWeapon()
    {
      int itemCount = _playerBackpack.Items.Count;
      if (itemCount == 0) return;

      int newIndex = (_currentWeaponIndex + 1) % itemCount;
      IWeapon newWeapon = null;

      for (int i = 0; i < itemCount; i++)
      {
        if (_playerBackpack.Items[newIndex] is IWeapon weapon)
        {
          newWeapon = weapon;
          _currentWeaponIndex = newIndex;
          break;
        }
        newIndex = (newIndex + 1) % itemCount; 
      }

      if (newWeapon == null)
        return; 

      Debug.Log(newWeapon.ItemData.Type);
      _playerAttacker.SwitchWeapon(newWeapon);
    }

    public void Disable() =>
      _inputService.OnChangeWeapon -= TrySetWeapon;
  }
}