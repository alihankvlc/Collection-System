using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.ItemSystem.Common
{
    public enum StackType
    {
        Increase,
        Decrease
    }

    [CreateAssetMenu(menuName = "_Project/Create Item/New Item")]
    public class Item : ScriptableObject
    {
        [SerializeField, InlineEditor] private ItemData _itemData;
        [SerializeField] private int _itemCount = 1;

        public int Count
        {
            get => _itemCount;
            private set => _itemCount = value;
        }

        public ItemData Data => _itemData;
        public event Action<int> OnChangeItemCount;

        public void Stack(StackType stackType, int stackAmount = 1)
        {
            if (_itemData.Stackable)
            {
                Count += stackType == StackType.Increase ? stackAmount : -stackAmount;
                OnChangeItemCount?.Invoke(_itemCount);
            }
        }
    }
}