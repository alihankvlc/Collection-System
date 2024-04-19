using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Framework;
using _Project.ItemSystem.Common;
using _Project.ItemSystem.Common.Database;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.InventorySystem.Common
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

    public class InventoryManager : Singleton<InventoryManager>, IInventoryHandler, ISlotVisualProvider
    {
        [Header("Inventory Toggle Settings")] [SerializeField]
        private KeyCode _toggleInventoryKey;

        [Header("Slot Settings")] [SerializeField, InlineEditor]
        private List<Slot> _slots = new();

        [SerializeField] private GameObject _slotInItemDisplay;

        [Inject] private IInventoryCheckable _inventoryCheckable;
        [Inject] private IInventoryProvider _inventoryProvider;

        public bool IsOpen { get; private set; }
        public static event Action<bool> OnInventoryEnable;

        public GameObject ItemDisplay
        {
            get => _slotInItemDisplay;
        }

        private void Update()
        {
            ToggleInventoryVisibility();
            if (Input.GetKeyDown(KeyCode.F))
            {
                AddItemToInventory(2);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                AddItemToInventory(1);
            }
        }


        public void AddItemToInventory(int itemId)
        {
            Item item = ItemDatabase.Instance.GetItemInDatabase(id: itemId);
            Slot slot = _slots.FirstOrDefault(r => r.Status == SlotStatus.Empty);

            if (slot != null)
                _inventoryProvider.AddItem(item, slot);
        }

        public void RemoveFromItem(int id)
        {
            _inventoryProvider.RemoveItem(id);
        }


        private void ToggleInventoryVisibility()
        {
            if (Input.GetKeyDown(_toggleInventoryKey))
            {
                IsOpen = !IsOpen;
                OnInventoryEnable?.Invoke(IsOpen);
            }
        }
    }
}