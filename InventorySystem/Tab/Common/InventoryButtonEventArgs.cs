using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _Project.InventorySystem.Tab.Common
{
    public class InventoryButtonEventArgs : MonoBehaviour, IPointerEnterHandler,
        IPointerExitHandler
    {
        [SerializeField] private InventoryTabType _tabType;
        [SerializeField] private GameObject _outline;
        [SerializeField, ReadOnly] private bool _isSelected;

        private Button _thisButton;

        public Action<InventoryButtonEventArgs> OnSelected;
        public InventoryTabType TabType => _tabType;

        private void Start()
        {
            _thisButton = GetComponent<Button>();
            _thisButton.onClick.AddListener(() => { OnSelected?.Invoke(this); });
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;

            _outline.SetActive(true);
            transform.DOScale(1.2f, 0.25f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;

            _outline.SetActive(false);
            transform.DOScale(1, 0.25f);
        }

        public void SetVisualEffect(bool isActive, ref InventoryTabType tabType)
        {
            if (isActive)
                tabType = _tabType;

            _outline.SetActive(isActive);
            _isSelected = isActive;
            transform.DOScale(isActive ? 1.2f : 1f, 0.25f);
        }
    }
}