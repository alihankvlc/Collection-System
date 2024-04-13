using System.Collections.Generic;
using _Project.Database.Singleton;
using UnityEngine;

namespace _Project.QuestSystem.Common.Database
{
    [CreateAssetMenu(menuName = "_Project/Database/Create Item Database")]
    public class QuestDatabase : SingletonForScriptableObject<QuestDatabase>
    {
        [SerializeField] private List<Quest> _quests = new();


        public Quest StartQuest<T>() where T : Quest
        {
            
            return null;
        }
    }
}