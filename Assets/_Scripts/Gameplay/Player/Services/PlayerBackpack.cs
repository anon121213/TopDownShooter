using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Gameplay.Items.Base;
using UnityEngine;

namespace _Scripts.Gameplay.Player.Services
{
  public class PlayerBackpack : IPlayerBackpack
  {
    private readonly List<IItem> _items = new();
    public IReadOnlyList<IItem> Items => _items;
    
    public void AddItem(IItem item, int count)
    {
      if (item == null)
        return;

      if (FindItem(item.ItemData.Type, out IItem findedItem))
      {
        findedItem.ItemData.Count.Value += count;
        return;
      }

      item.ItemData.Count.Value += count;
      _items.Add(item);
    }

    public bool FindItem(ItemType type, out IItem findedItem)
    {
      findedItem = null;

      foreach (var item in Items.Where(item => item.ItemData.Type == type))
      {
        findedItem = item;
        return true;
      }

      return false;
    }

    public void RemoveItem(ItemType type, int count)
    {
      if (FindItem(type, out IItem item)) 
        item.ItemData.Count.Value = Mathf.Clamp(item.ItemData.Count.Value - count, 0, Int32.MaxValue);
    }
  }

  public interface IPlayerBackpack
  {
    IReadOnlyList<IItem> Items { get; }
    void AddItem(IItem item, int count);
    bool FindItem(ItemType type, out IItem findedItem);
    void RemoveItem(ItemType type, int count);
  }
}