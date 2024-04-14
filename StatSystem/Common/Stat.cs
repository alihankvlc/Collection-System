using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.StatSystem.Common
{
    public enum StatType
    {
        None,
        Health,
        Stamina
    }

    public interface IStatObserver
    {
        public void OnNotify(StatType type, int  groupId,float currentValue, float maxValue);
    }

    public abstract class Stat : SerializedScriptableObject
    {
        [Header("General ID Settings")] [SerializeField]
        private StatType _type = StatType.None;

        [SerializeField] private int _groupId;

        [SerializeField] private float _maxValue = 100;
        
        [ReadOnly, SerializeField] private float _currentValue = 100;
        [ReadOnly, SerializeField] private float _statChangedAmount;

        private List<IStatObserver> _observers = new();

        public StatType Type => _type;
        public int GroupId => _groupId;

        public float CurrentValue
        {
            get => _currentValue;
            protected set => _currentValue = value;
        }

        public float MaxValue
        {
            get => _maxValue;
            protected set => _maxValue = value;
        }

        public float ChangedAmount => _statChangedAmount;

        public virtual float Modify
        {
            get => _currentValue;
            set
            {
                float previousValue = _currentValue;
                float newValue = Mathf.Clamp(value, 0, _maxValue);

                if (newValue != _currentValue)
                {
                    bool isIncrease = newValue > previousValue;
                    _currentValue = newValue;
                    _statChangedAmount = isIncrease ? newValue - previousValue : (previousValue - newValue) * 1;
                    NotifyObservers();
                }
            }
        }

        public void RegisterObserver(IStatObserver observer) => _observers.Add(observer);
        protected void NotifyObservers() => _observers.ForEach(r => r.OnNotify(_type,_groupId, Modify, _maxValue));
    }
}