using System;
using System.Collections.Generic;
using _Project.InventorySystem.ToolBelt;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using Zenject;

namespace _Project.InventorySystem.Common
{
    [System.Serializable]
    public class ToolBeltManager : MonoBehaviour
    {
        [SerializeField] private List<ToolBeltSlot> _slots = new();
        [SerializeField] private int _activeSlotIndex = 0;

        public int _toolbeltSize;

        [Inject] private INavigate _navigate;

        private void Start()
        {
            _toolbeltSize = _slots.Count + 1;
            _navigate.OnNavigate(_slots[_activeSlotIndex]);
        }

        private void Update()
        {
            if (InventoryManager.Instance.IsOpen) return;
            NavigateKey();
            NavigateScroll();
        }

        private void NavigateKey()
        {
            for (int i = 0; i < _toolbeltSize; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i - 1))
                {
                    _activeSlotIndex = i - 1;
                    if (_activeSlotIndex >= 0 && _activeSlotIndex < _slots.Count)
                    {
                        _navigate.OnNavigate(_slots[_activeSlotIndex]);
                    }
                }
            }
        }

        private void NavigateScroll()
        {
            Vector2 scrollValue = Input.mouseScrollDelta;
        }
    }
}