using _Project.ItemSystem.Common;
using _Project.ItemSystem.Common.Display;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.InventorySystem.Common
{
    public enum SlotStatus
    {
        Empty,
        Occupied
    }

    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        [Header("ID Settings")] [SerializeField]
        private int _slotIndex;

        [SerializeField, ReadOnly] private SlotStatus _status;

        [Header("Slot In Item Settings")] [SerializeField, ReadOnly]
        private Item _slotInItem;

        [SerializeField, ReadOnly] private int _slotInItemCount;


        public SlotStatus Status => _status;
        public int Index => _slotIndex;
        public Item SlotInItem => _slotInItem;
        public int SlotInItemCount => _slotInItemCount;

        [Inject] private ISlotVisualProvider _visualProvider;
        [Inject] private IInventoryProvider _inventoryProvider;

        public void AddItemToSlot(Item item, bool isCreated = true)
        {
            CreateItemDisplay(item, isCreated);
            UpdateSlot(SlotStatus.Occupied, item, item.Count);
        }

        public void RemoveFromItem()
        {
            UpdateSlot(SlotStatus.Empty, null, 0);
        }

        private void UpdateSlot(SlotStatus status, Item item, int slotInItemCount = 1)
        {
            _status = status;
            _slotInItem = item;

            _slotInItemCount = slotInItemCount;
        }

        private void CreateItemDisplay(Item item, bool isCreated)
        {
            if (!isCreated) return;
            GameObject visualProvider = Instantiate(_visualProvider.ItemDisplay, transform);
            ItemBehaviour itemBehaviour = visualProvider.GetComponent<ItemBehaviour>();
            itemBehaviour?.Init(item);
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedObject = eventData.pointerDrag;

            if (droppedObject != null)
            {
                DraggableItem draggableDropObject = droppedObject.GetComponent<DraggableItem>();

                if (draggableDropObject != null)
                {
                    InventorySlot parentAfterSlot = draggableDropObject.ParentAfterSlot;
                    Item parentAfterItem = parentAfterSlot?.SlotInItem;

                    if (_status == SlotStatus.Empty)
                    {
                        parentAfterSlot.RemoveFromItem();
                        draggableDropObject.ParentAfterDrag = transform;

                        AddItemToSlot(parentAfterItem, false);
                    }
                    else
                    {
                        Item previousDraggableItem = null;

                        if (transform.childCount <= 0)
                            return;

                        Transform itemInSlot = transform.GetChild(0);
                        Transform previousTransform = draggableDropObject.ParentAfterDrag;

                        if (draggableDropObject.ParentAfterSlot != null)
                            previousDraggableItem = draggableDropObject.ParentAfterSlot.SlotInItem;

                        parentAfterSlot?.RemoveFromItem();
                        parentAfterSlot?.AddItemToSlot(_slotInItem, false);
                        draggableDropObject.ParentAfterDrag = transform;

                        AddItemToSlot(previousDraggableItem, false);
                        itemInSlot.SetParent(previousTransform);
                        itemInSlot.SetAsLastSibling();
                    }
                }
            }
        }
    }
}