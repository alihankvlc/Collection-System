using System;
using _Project.InventorySystem.Common;
using UnityEngine;

namespace _Project.InventorySystem.Other
{
    public class CursorSettings : MonoBehaviour
    {
        private void Update()
        {
            Cursor.visible = InventoryManager.Instance.IsOpen;
            Cursor.lockState = InventoryManager.Instance.IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}