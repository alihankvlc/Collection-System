using System;
using _Project.InventorySystem.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.ItemSystem.Common.Display
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private Image _itemIcon;
        [HideInInspector] public Transform ParentAfterDrag;
        [HideInInspector] public InventorySlot ParentAfterSlot;
        private CanvasGroup _canvasGroup;

        private const float _transparencyValue = 0.25f;
        private const float _defaulthValue = 1f;


        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ParentAfterSlot = GetComponentInParent<InventorySlot>();
            ParentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            _itemIcon.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            _canvasGroup.alpha = _transparencyValue;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(ParentAfterDrag);
            _itemIcon.raycastTarget = true;
            _canvasGroup.alpha = _defaulthValue;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}