using System;
using System.Collections.Generic;
using System.Linq;
using _Project.InventorySystem.Common;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.InventorySystem.Tab.Common
{
    public enum TabType
    {
        None,
        Quest,
        Inventory,
        Skill,
        Crafting
    }

    public class TabWindowManager : Framework.Singleton<TabWindowManager>
    {
        [SerializeField, ReadOnly] private TabButtonEventArgs _previousTabButtonEventArgs;
        [SerializeField] private List<TabButtonEventArgs> _tabButtonEventArgs = new();

        [SerializeField] private GameObject _tabParent;
        [SerializeField] private GameObject _quest;
        [SerializeField] private GameObject _inventory;
        [SerializeField] private GameObject _crafting;
        [SerializeField] private GameObject _skills;
        [SerializeField] private GameObject _toolbelt;


        private void Start()
        {
            _tabButtonEventArgs.ForEach(r => r.OnNotifySelectedTab += SelectedTab);
            InventoryManager.OnInventoryEnable += OnInventoryEnable;
        }


        private void SelectedTab(TabButtonEventArgs tabEventArgs)
        {
            if (_previousTabButtonEventArgs != null)
            {
                _previousTabButtonEventArgs.IsSelected(false);
                _previousTabButtonEventArgs = null;
            }


            _previousTabButtonEventArgs = tabEventArgs;

            _previousTabButtonEventArgs.IsSelected(true);
            EnableTabWindow(tabEventArgs.Type);
        }

        private void EnableTabWindow(TabType type)
        {
            _toolbelt.SetActive(type == TabType.Inventory);
            _inventory.SetActive(type == TabType.Inventory);
            _quest.SetActive(type == TabType.Quest);
            _skills.SetActive(type == TabType.Skill);
            _crafting.SetActive(type == TabType.Crafting);
        }

        private void OnInventoryEnable(bool isEnable)
        {
            TabButtonEventArgs inventoryTab = _tabButtonEventArgs.FirstOrDefault(r => r.Type == TabType.Inventory);

            _previousTabButtonEventArgs = isEnable ? inventoryTab : null;

            _previousTabButtonEventArgs?.IsSelected(isEnable);
            EnableTabWindow(isEnable ? TabType.Inventory : TabType.None);

            if (!isEnable)
            {
                _previousTabButtonEventArgs?.IsSelected(false);
                _previousTabButtonEventArgs = null;
                _tabButtonEventArgs.ForEach(r => r.IsSelected(false));
            }

            _tabParent.SetActive(isEnable);
        }

        private void OnDisable()
        {
            _tabButtonEventArgs.ForEach(r => r.OnNotifySelectedTab -= SelectedTab);
            InventoryManager.OnInventoryEnable -= OnInventoryEnable;
        }
    }
}