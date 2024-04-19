using System;
using _Project.InventorySystem.Common;
using DG.Tweening;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.ItemSystem.Common
{
    public class ItemVisualProvider : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _itemCountTextMesh;
        [SerializeField] private Button _interactButton;
        [SerializeField] private Image _itemImage;

        [SerializeField] private Item _item;

        public void Init(Item item)
        {
            _item = item;
            _itemCountTextMesh.SetText(item.Count.ToString());
            _itemImage.sprite = item.Data.Icon;
            _item.OnChangeItemCount += UpdateCountTextMesh;
        }

        public void UpdateCountTextMesh(int value)
        {
            _itemCountTextMesh.SetText(value.ToString());
        }

        /// <summary>
        /// Düzenleneccek...
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            SlotInItemDetailWindow.Instance.Init(_item, true);
            _itemImage.transform.DOScale(0.85f, 0.15f).OnComplete(
                () => _itemImage.transform.DOScale(1f, 0.15f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SlotInItemDetailWindow.Instance.Init(_item, false);
        }

        private void OnDestroy()
        {
            _item.OnChangeItemCount -= UpdateCountTextMesh;
        }
    }
}