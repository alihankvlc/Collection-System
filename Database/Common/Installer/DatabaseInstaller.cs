using _Project.ItemSystem.Common.Database;
using _Project.StatSystem.Common.Database;
using UnityEngine;
using Zenject;

namespace _Project.Database.Common.Installer
{
    public class DatabaseInstaller : MonoInstaller
    {
        [SerializeField] private ItemDatabase _itemDatabase;
        [SerializeField] private StatDatabase _statDatabase;

        public override void InstallBindings()
        {
            Container.Bind<IItemDatabaseHandler>().FromInstance(_itemDatabase).AsSingle();
            Container.Bind<IStatDatabaseHandler>().FromInstance(_statDatabase).AsSingle();
        }
    }
}