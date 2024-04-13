using UnityEngine;

namespace _Project.QuestSystem.Common
{
    public abstract class Quest : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField, Multiline] private string _description;
        [SerializeField] private bool _completed;
    }
}