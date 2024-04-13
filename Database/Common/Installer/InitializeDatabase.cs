using System;
using _Project.ItemSystem.Common.Database;
using _Project.StatSystem.Common.Database;
using UnityEngine;
using Zenject;

namespace _Project.Database.Common.Installer
{
    public class InitializeDatabase : MonoBehaviour
    {
        [Inject] private IItemDatabaseHandler _itemDatabaseHandler;

        private void Awake()
        {
            _itemDatabaseHandler.Init();
        }
    }
}