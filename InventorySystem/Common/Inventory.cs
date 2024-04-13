using System;
using System.Collections.Generic;
using System.Linq;
using _Project.ItemSystem.Common;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.InventorySystem.Common
{
    public interface IInventoryCheckable
    {
        public bool IsFull { get; }
        public bool HasItem(int id);
    }

    public interface IInventoryProvider
    {
        public void AddItem(Item item, InventorySlot slot);
        public void RemoveItem(int itemId);
        public void MoveToSlotInItem(int itemId, int previousSlot, int newSlotIndex, int slotInItemCount);
    }


    [System.Serializable]
    public class InventorySlotEntry //Envanteri savelemek istediğimde kullanıcam...
    {
        public SlotStatus SlotStatus;
        public int SlotIndex;
        public int SlotInItemId;
        public int SlotInItemCount;
    }

    public class Inventory : SerializedMonoBehaviour, IInventoryProvider, IInventoryCheckable
    {
        [SerializeField, InlineEditor] private List<Item> _items = new();

        [SerializeField] private int _capacity;
        [SerializeField, ReadOnly] private int _currentWeight;

        public bool IsFull { get; private set; }
        public bool HasItem(int id) => _items.Any(r => r.Data.Id == id);

        public static event Action OnInventoryCapacityOut;
        public static event Action<int> OnChangeInventoryWeight;

        public void AddItem(Item item, InventorySlot slot)
        {
            if (_currentWeight >= _capacity) OnInventoryCapacityOut?.Invoke();

            if (HasItem(item.Data.Id) && item.Data.Stackable)
            {
                _currentWeight += item.Data.Weight;
                item.Stack(StackType.Increase);
                OnChangeInventoryWeight?.Invoke(_currentWeight);
                return;
            }

            _items.Add(item);
            slot.AddItemToSlot(item);

            _currentWeight += item.Data.Weight;
            OnChangeInventoryWeight?.Invoke(_currentWeight);
        }

        public void RemoveItem(int itemId)
        {
            Item item = _items.FirstOrDefault(r => r.Data.Id == itemId);
            if (_items.Contains(item))
            {
                if (item.Data.Stackable)
                {
                    item.Stack(StackType.Decrease);
                    _currentWeight -= item.Data.Weight;
                    OnChangeInventoryWeight?.Invoke(_currentWeight);
                    if (item.Count < 1)
                    {
                        _currentWeight = Mathf.Max(0, _currentWeight - item.Data.Weight);
                        OnChangeInventoryWeight?.Invoke(_currentWeight);
                        _items.Remove(item);
                        return;
                    }

                    return;
                }

                _currentWeight = Mathf.Max(0, _currentWeight - item.Data.Weight);
                OnChangeInventoryWeight?.Invoke(_currentWeight);
                _items.Remove(item);
            }
        }

        public void MoveToSlotInItem(int itemId, int previousSlot, int newSlotIndex, int slotInItemCount)
        {
        }

        public InventorySlotEntry AddSlotEntry(int itemId, int itemCount, int slotIndex, SlotStatus status)
        {
            return new InventorySlotEntry
            {
                SlotStatus = status,
                SlotIndex = slotIndex,
                SlotInItemId = itemId,
                SlotInItemCount = itemCount,
            };
        }
    }
}