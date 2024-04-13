using System.Collections.Generic;
using System.Linq;
using _Project.Database.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.StatSystem.Common.Database
{
    public interface IStatDatabaseHandler
    {
        public void Init(IStatObserver observer);
    }

    [CreateAssetMenu(menuName = "_Project/Database/Crate Stat Database")]
    public class StatDatabase : SingletonForScriptableObject<StatDatabase>,IStatDatabaseHandler
    {
        [SerializeField, InlineEditor] private List<Stat> _datas = new();

        public void Init(IStatObserver observer)
        {
            _datas.ForEach(r => r.RegisterObserver(observer));
        }

        public Stat GetStatInDatabase<T>(int groupId) where T : Stat
        {
            Stat stat = _datas.FirstOrDefault(r => r is T && r.GroupId == groupId);

            if (stat != null)
                return stat;

            Debug.Log($"{groupId} ??? null ???");
            return null;
        }
    }
}