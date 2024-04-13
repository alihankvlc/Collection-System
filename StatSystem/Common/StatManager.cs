using System;
using System.Runtime.InteropServices.ComTypes;
using _Project.StatSystem.Common.Database;
using UnityEngine;
using Zenject;

namespace _Project.StatSystem.Common
{
    public interface IStatProvider
    {
        public Stat GetStat<T>(int groupId) where T : Stat;
    }

    public class StatManager : MonoBehaviour, IStatObserver, IStatProvider
    {
        [Inject] private IStatDatabaseHandler _statDatabaseHandler;

        public delegate void NotifyStatDelegate(StatType type, int groupId, int currentValue, int maxValue);

        public static event NotifyStatDelegate OnNotiftStat;

        private void Start()
        {
            _statDatabaseHandler.Init(this);
        }

        public void OnNotify(StatType type, int groupId, int currentValue, int maxValue)
        {
            OnNotiftStat?.Invoke(type, groupId, currentValue, maxValue);
        }

        public Stat GetStat<T>(int groupId) where T : Stat
        {
            Stat stat = StatDatabase.Instance.GetStatInDatabase<T>(groupId);
            if (stat != null)
            {
                return stat;
            }

            Debug.Log($"{typeof(T).Name} GetStat ??!! Null");
            return null;
        }
    }
}