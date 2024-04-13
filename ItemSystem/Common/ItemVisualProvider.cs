using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.ItemSystem.Common
{
    public class ItemVisualProvider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemCountTextMesh;
        [SerializeField] private Image _itemImage;

        private Item _item;

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

        private void OnDestroy()
        {
            _item.OnChangeItemCount -= UpdateCountTextMesh;
        }
    }
}