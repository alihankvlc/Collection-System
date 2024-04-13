using UnityEngine;

namespace _Project.ItemSystem.Common.Display
{
    public class ItemBehaviour : MonoBehaviour
    {
        [SerializeField] private ItemVisualProvider _visualProvider;
        
        public void Init(Item item)
        {
            _visualProvider.Init(item);
        }
    }
}