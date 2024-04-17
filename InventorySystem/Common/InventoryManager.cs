using System.Collections.Generic;
using System.Linq;
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
        [Header("Inventory Window"), Space] [SerializeField]
        private GameObject _inventoryWindow;

        [Header("Inventory Toggle Settings")] [SerializeField]
        private KeyCode _toggleInventoryKey;

        [Header("Slot Settings")] [SerializeField, InlineEditor]
        private List<Slot> _slots = new();

        [SerializeField] private GameObject _slotInItemDisplay;

        [Inject] private IInventoryCheckable _inventoryCheckable;
        [Inject] private IInventoryProvider _inventoryProvider;

        public bool IsOpen { get; private set; }

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
            Slot emptySlot = _slots.FirstOrDefault(r =>
                r.Status == SlotStatus.Empty);

            if (emptySlot != null)
                _inventoryProvider.AddItem(item, emptySlot);
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
                _inventoryWindow.SetActive(IsOpen);
            }
        }
    }
}