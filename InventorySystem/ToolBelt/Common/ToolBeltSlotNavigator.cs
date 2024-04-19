using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.InventorySystem.ToolBelt.Common
{
    public interface INavigate
    {
        public void OnNavigate(ToolBeltSlot slot);
    }

    public class ToolBeltSlotNavigator : MonoBehaviour, INavigate
    {
        [SerializeField] private float _onhighlightedScale = 1.1f;
        [SerializeField] private float _navigateChangeDuration = 0.25f;
        [SerializeField, ReadOnly] private ToolBeltSlot _previousNavigateSlot;

        private const int _slotDefaultScaleValue = 1;

        private readonly int _navigateHashId = Animator.StringToHash("OnNavigate");

        public void OnNavigate(ToolBeltSlot slot)
        {
            if (_previousNavigateSlot != null)
            {
                PlayVisualEffect(_previousNavigateSlot.transform, _slotDefaultScaleValue,
                    _navigateChangeDuration);
                _previousNavigateSlot.GetComponent<Animator>().SetBool(_navigateHashId, false);

                _previousNavigateSlot = null;
            }

            _previousNavigateSlot = slot;
            PlayVisualEffect(_previousNavigateSlot.transform, _onhighlightedScale, _navigateChangeDuration);
            _previousNavigateSlot.GetComponent<Animator>().SetBool(_navigateHashId, true);
        }

        private void PlayVisualEffect(Transform target, float endValue, float duration)
        {
            target.transform.DOScale(endValue, duration);
        }
    }
}