using UnityEngine;

namespace _Project.ItemSystem.Common
{
    public enum ItemType
    {
        None,
        Weapon,
        Medkit,
        Food,
        Drink,
        Resources,
        Ammo
    }

    public abstract class ItemData : ScriptableObject
    {
        [Header("ID Settings")] [SerializeField]
        private ItemType _itemType;

        [SerializeField] private int _itemId;

        [Header("Display Settings")] [SerializeField]
        private string _itemName;

        [SerializeField] private string _itemDescription;
        [SerializeField] private Sprite _itemIcon;

        [Header("Other")] [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private int _weight;
        [SerializeField] private bool _isStackable;

        public ItemType Type
        {
            get => _itemType;
            protected set => _itemType = value;
        }

        public int Id => _itemId;
        public string Name => _itemName;
        public string Description => _itemDescription;
        public Sprite Icon => _itemIcon;
        public bool Stackable => _isStackable;
        public int Weight => _weight;
    }
}