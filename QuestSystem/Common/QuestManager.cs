using System.Collections.Generic;
using _Project.QuestSystem.Common.SubQuests;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.QuestSystem.Common
{
    [System.Serializable]
    public class QuestBinding<T> where T : Quest
    {
        [SerializeField, InlineEditor] private List<T> _quests = new();
        [SerializeField] private T _activeQuest;
    }

    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestBinding<MainQuest> _mainQuests;
        [SerializeField] private QuestBinding<HuntingQuest> _huntingQuests;
        [SerializeField] private QuestBinding<SideQuest> _sideQuests;
        [SerializeField] private QuestBinding<ExplorationQuest> _explorationQuests;
    }
}