using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.InventorySystem.Tab.Common
{
    public enum InventoryTabType
    {
        None,
        All,
        Weapon,
        Ammo,
        Medkit,
        Herbs,
        Resources,
        Armor
    }

    public class InventoryTabManager : Singleton<InventoryTabManager>
    {
        [SerializeField] private TextMeshProUGUI _headerTextMesh;
        public static Action<string> OnHighlightedChangeHeader;

        [SerializeField] private List<InventoryButtonEventArgs> _inventoryTabs = new();
        [SerializeField] private InventoryTabType _activeTabType;
        [SerializeField] private InventoryButtonEventArgs _previousTabEventArg;

        private void Start()
        {
            _activeTabType = InventoryTabType.All;
            _previousTabEventArg.SetVisualEffect(true, ref _activeTabType);
            _inventoryTabs.ForEach(r => r.OnSelected += IsSelectedTab);
        }


        private void OnDisable()
        {
            _inventoryTabs.ForEach(r => r.OnSelected -= IsSelectedTab);
        }

        public void HighlightedHeaderInfo(string headerName)
        {
            _headerTextMesh.SetText(headerName);
        }

        public void IsSelectedTab(InventoryButtonEventArgs tabButtonEventArg)
        {
            if (_previousTabEventArg != null && tabButtonEventArg != _previousTabEventArg)
            {
                _previousTabEventArg.SetVisualEffect(false, ref _activeTabType);
            }

            _previousTabEventArg = tabButtonEventArg;
            _previousTabEventArg.SetVisualEffect(true, ref _activeTabType);

            _headerTextMesh.SetText(tabButtonEventArg.TabType.ToString());
        }
    }
}