using System;
using System.Collections.Generic;
using System.Linq;
using _Project.InventorySystem.Common;
using _Project.ItemSystem.Common.Database;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.ItemSystem.Common
{
    public interface IInventoryHandler
    {
        public void AddItemToInventory(int id);
        public void RemoveFromItem(int id);
    }

    public interface ISlotVisualProvider
    {
        public GameObject ItemDisplay { get; }
    }

    public class InventoryManager : MonoBehaviour, IInventoryHandler, ISlotVisualProvider
    {
        [SerializeField, InlineEditor] private List<InventorySlot> _slots = new();
        [SerializeField] private GameObject _itemDisplay;

        [Inject] private IInventoryCheckable _inventoryCheckable;
        [Inject] private IInventoryProvider _inventoryProvider;

        public GameObject ItemDisplay
        {
            get => _itemDisplay;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
 
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RemoveFromItem(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddItemToInventory(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                RemoveFromItem(2);
            }
        }


        public void AddItemToInventory(int itemId)
        {
            Item item = ItemDatabase.Instance.GetItemInDatabase(id: itemId);
            InventorySlot emptySlot = _slots.FirstOrDefault(r =>
                r.Status == SlotStatus.Empty);

            if (emptySlot != null)
                _inventoryProvider.AddItem(item, emptySlot);
        }

        public void RemoveFromItem(int id)
        {
            _inventoryProvider.RemoveItem(id);
        }
    }
}