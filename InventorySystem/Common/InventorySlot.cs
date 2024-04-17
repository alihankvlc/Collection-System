using System;
using _Project.ItemSystem.Common;
using _Project.ItemSystem.Common.Display;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.InventorySystem.Common
{
    public class InventorySlot : Slot
    {
        private void Start()
        {
            _slotType = SlotType.Inventory;
        }
        public override void AddItemToSlot(Item item, bool isCreated = true)
        {
            base.AddItemToSlot(item, isCreated);
            CreateItemDisplay(item, isCreated, transform);
        }
    }
}