using _Project.InventorySystem.Tab.Common;
using _Project.ItemSystem.Common;
using UnityEngine;
using Zenject;

namespace _Project.InventorySystem.Common.Installer
{
    public class InventoryService : MonoInstaller
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private InventoryManager _manager;
        [SerializeField] private ToolBeltSlotNavigator _navigator;
        public override void InstallBindings()
        {
            Container.Bind<IInventoryProvider>().FromInstance(_inventory).AsSingle();
            Container.Bind<IInventoryCheckable>().FromInstance(_inventory).AsSingle();
            Container.Bind<IInventoryHandler>().FromInstance(_manager).AsSingle();
            Container.Bind<ISlotVisualProvider>().FromInstance(_manager).AsSingle();
            Container.Bind<INavigate>().FromInstance(_navigator).AsSingle();
        }
    }
}