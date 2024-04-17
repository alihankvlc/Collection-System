using System;
using _Project.InventorySystem.Common;
using _Project.ItemSystem.Common;
using _Project.ItemSystem.Common.Display;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.InventorySystem.ToolBelt
{
    public class ToolBeltSlot : Slot
    {
        [SerializeField] private Transform _slotContentPlaceHolder;

        public Transform ContentPlaceHolder => _slotContentPlaceHolder;

        private void Start()
        {
            _slotType = SlotType.ToolBelt;
        }

        public override void AddItemToSlot(Item item, bool isCreated = true)
        {
            base.AddItemToSlot(item, isCreated);
            CreateItemDisplay(item, isCreated, _slotContentPlaceHolder);
        }

        public override void RemoveFromItem()
        {
            base.RemoveFromItem();
        }
    }
}