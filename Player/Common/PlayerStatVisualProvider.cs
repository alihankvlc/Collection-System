using System;
using _Project.StatSystem.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Player.Common
{
    public class PlayerStatVisualProvider : MonoBehaviour
    {
        [SerializeField] private GameObject _uiSliderGameObject;

        private void Start()
        {
            StatManager.OnNotiftStat += UpdateStatVisualProvider;
        }

        private void UpdateStatVisualProvider(StatType type, int groupId, float currentValue, float maxValue)
        {
            switch (type)
            {
                case StatType.Stamina:

                    _uiSliderGameObject.transform.DOScaleX(currentValue / 100, 0f);
                    _uiSliderGameObject.transform.parent.gameObject.SetActive(currentValue != maxValue);
                    break;
            }
        }

        private void OnDestroy()
        {
            StatManager.OnNotiftStat -= UpdateStatVisualProvider;
        }
    }
}