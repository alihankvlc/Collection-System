using _Project.InventorySystem.ToolBelt;
using _Project.ItemSystem.Common;
using _Project.ItemSystem.Common.Display;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.InventorySystem.Common
{
    public enum SlotType
    {
        Inventory,
        ToolBelt
    }

    public enum SlotStatus
    {
        Empty,
        Occupied
    }

    public abstract class Slot : MonoBehaviour, IDropHandler
    {
        [Header("ID Settings")] [SerializeField, ReadOnly]
        protected SlotType _slotType;

        [SerializeField, ReadOnly] protected SlotStatus _status;
        [SerializeField] protected int _slotIndex;


        [Header("Slot In Item Settings")] [SerializeField, ReadOnly]
        protected Item _slotInItem;

        [SerializeField, ReadOnly] protected int _slotInItemCount;


        [Inject] protected ISlotVisualProvider _visualProvider;
        [Inject] protected IInventoryProvider _inventoryProvider;

        public virtual SlotType SlotType
        {
            get => _slotType;
            protected set => _slotType = value;
        }

        public virtual SlotStatus Status
        {
            get => _status;
            protected set => _status = value;
        }

        public virtual int Index
        {
            get => _slotIndex;
            protected set => _slotIndex = value;
        }

        public virtual Item SlotInItem
        {
            get => _slotInItem;
            protected set => _slotInItem = value;
        }

        public virtual int SlotInItemCount
        {
            get => _slotInItemCount;
            protected set => _slotInItemCount = value;
        }

        public virtual void AddItemToSlot(Item item, bool isCreated = true)
        {
            UpdateSlot(SlotStatus.Occupied, item, item.Count);
        }

        public virtual void RemoveFromItem()
        {
            UpdateSlot(SlotStatus.Empty, null, 0);
        }

        protected void UpdateSlot(SlotStatus status, Item item, int slotInItemCount = 1)
        {
            _status = status;
            _slotInItem = item;

            _slotInItemCount = slotInItemCount;
        }

        protected void CreateItemDisplay(Item item, bool isCreated, Transform initTransform)
        {
            if (!isCreated) return;
            GameObject visualProvider = Instantiate(_visualProvider.ItemDisplay, initTransform);
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
                    Slot parentAfterSlot = draggableDropObject.ParentAfterSlot;
                    Item parentAfterItem = parentAfterSlot?.SlotInItem;

                    Transform targetToolBeltSlot = null;
                    ToolBeltSlot toolBeltSlot = null;

                    if (_slotType == SlotType.ToolBelt)
                    {
                        targetToolBeltSlot = transform;
                        toolBeltSlot = targetToolBeltSlot.GetComponent<ToolBeltSlot>();
                    }

                    if (_status == SlotStatus.Empty && parentAfterSlot != null)
                    {
                        parentAfterSlot.RemoveFromItem();
                        draggableDropObject.ParentAfterDrag = _slotType == SlotType.Inventory
                            ? transform
                            : toolBeltSlot.ContentPlaceHolder;

                        AddItemToSlot(parentAfterItem, false);
                    }
                    else
                    {
                        Item previousDraggableItem = null;

                        bool hasChildInItem = _slotType == SlotType.Inventory
                            ? transform.childCount <= 0
                            : toolBeltSlot.ContentPlaceHolder.transform.childCount <= 0;

                        if (hasChildInItem)
                            return;

                        Transform itemInSlot = _slotType == SlotType.Inventory
                            ? transform.GetChild(0)
                            : toolBeltSlot.ContentPlaceHolder.GetChild(0);

                        Transform previousTransform = draggableDropObject.ParentAfterDrag;

                        if (draggableDropObject.ParentAfterSlot != null)
                            previousDraggableItem = draggableDropObject.ParentAfterSlot.SlotInItem;

                        parentAfterSlot?.RemoveFromItem();
                        parentAfterSlot?.AddItemToSlot(_slotInItem, false);
                        draggableDropObject.ParentAfterDrag = _slotType == SlotType.Inventory
                            ? transform
                            : toolBeltSlot.ContentPlaceHolder;

                        AddItemToSlot(previousDraggableItem, false);
                        itemInSlot.SetParent(previousTransform);
                        itemInSlot.SetAsLastSibling();
                    }
                }
            }
        }
    }
}