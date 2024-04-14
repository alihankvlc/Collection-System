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

    public class InventoryManager : MonoBehaviour, IInventoryHandler, ISlotVisualProvider
    {
        [Header("Inventory Window"), Space] [SerializeField]
        private GameObject _inventoryWindow;

        [Header("Inventory Toggle Settings")] [SerializeField]
        private KeyCode _toggleInventoryKey;

        [Header("Inventory Slot Settings")] [SerializeField, InlineEditor]
        private List<InventorySlot> _slots = new();

        [SerializeField] private GameObject _slotInItemDisplay;

        [Inject] private IInventoryCheckable _inventoryCheckable;
        [Inject] private IInventoryProvider _inventoryProvider;

        private bool _isShow;

        public GameObject ItemDisplay
        {
            get => _slotInItemDisplay;
        }

        public bool InventoryIsOpen
        {
            get => _isShow;
            private set => _isShow = value;
        }

        private void Update()
        {
            ToggleInventoryVisibility();
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

        private void ToggleInventoryVisibility()
        {
            if (Input.GetKeyDown(_toggleInventoryKey))
            {
                _isShow = !_isShow;
                _inventoryWindow.SetActive(_isShow);
            }
        }
    }
}