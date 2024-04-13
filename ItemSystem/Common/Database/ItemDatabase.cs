using System.Collections.Generic;
using System.Linq;
using _Project.Database.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.ItemSystem.Common.Database
{
    public interface IItemDatabaseHandler
    {
        public void Init();
    }

    [CreateAssetMenu(menuName = "_Project/Database/Create Item Database")]
    public class ItemDatabase : SingletonForScriptableObject<ItemDatabase>, IItemDatabaseHandler
    {
        [SerializeField, InlineEditor] private List<Item> _datas = new();
        [SerializeField, ReadOnly] private Dictionary<int, Item> _cache = new();

        public void Init()
        {
            _cache = _datas.ToDictionary(r => r.Data.Id);
        }

        public Item GetItemInDatabase(int id)
        {
            if (_cache.TryGetValue(id, out Item existingItem))
                return existingItem;

            Debug.Log($"{id} ??? null ???");
            return null;
        }
    }
}