using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.InventorySystem.Tab.Common
{
    public class TabButtonEventArgs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TabType _tabType;
        [SerializeField] private GameObject _outlineObject;

        private Button _interactButton;

        private const float _defaultScaleValue = 1f;
        private const float _highlightScaleValue = 1.15f;
        private const float _scaleDuration = 0.25f;

        private bool _isSelected;

        public TabType Type => _tabType;
        public event Action<TabButtonEventArgs> OnNotifySelectedTab;

        private void Start()
        {
            _interactButton = GetComponent<Button>();
            _interactButton?.onClick.AddListener(() =>
            {
                if (_isSelected) return;
                OnNotifySelectedTab?.Invoke(this);
            });
        }

        public void IsSelected(bool isSelect)
        {
            _isSelected = isSelect;

            transform.DOScale(isSelect ? _highlightScaleValue : _defaultScaleValue, _scaleDuration);
            _outlineObject.SetActive(isSelect);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _outlineObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            _outlineObject.SetActive(false);
        }
    }
}