using System;
using _Project.Framework;
using _Project.ItemSystem.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.InventorySystem.Common
{
    public interface IItemDetailView
    {
        public void Init(Item item, bool d);
        public void SetName(string name);
        public void SetDescription(string description);
        public void SetWeight(int weight);
        public void SetIcon(Sprite icon);
    }
/// <summary>
/// Sınıf düzenlenecek...
/// </summary>
    public class SlotInItemDetailWindow : Singleton<SlotInItemDetailWindow>, IItemDetailView
    {
        [SerializeField] private GameObject _window;

        [SerializeField] private TextMeshProUGUI _itemNameTextMesh;
        [SerializeField] private TextMeshProUGUI _itemDescriptionTextMesh;
        [SerializeField] private TextMeshProUGUI _itemWeightTextMesh;
        [SerializeField] private Image _itemIconContainer;

        private void Update()
        {
            if (!InventoryManager.Instance.IsOpen && _window.activeInHierarchy)
            {
                _window.SetActive(false);
            }
        }

        public void Init(Item item, bool isActive)
        {
            _window.SetActive(isActive);

            SetName(item.Data.Name);
            SetDescription(item.Data.Description);
            SetWeight(item.Data.Weight);
            SetIcon(item.Data.Icon);
        }

        public void SetName(string name) => _itemNameTextMesh.SetText(name);
        public void SetDescription(string desc) => _itemDescriptionTextMesh.SetText(desc);
        public void SetWeight(int weight) => _itemWeightTextMesh.SetText($"{weight}.0 lbs");
        public void SetIcon(Sprite icon) => _itemIconContainer.sprite = icon;
    }
}