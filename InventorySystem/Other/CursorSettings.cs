using System;
using _Project.InventorySystem.Common;
using UnityEngine;

namespace _Project.InventorySystem.Other
{
    public class CursorSettings : MonoBehaviour
    {
        [SerializeField] private GameObject _crosshairObject;

        private void Update()
        {
            Cursor.visible = InventoryManager.Instance.IsOpen;
            Cursor.lockState = InventoryManager.Instance.IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
            _crosshairObject.SetActive(!InventoryManager.Instance.IsOpen);
        }
    }
}