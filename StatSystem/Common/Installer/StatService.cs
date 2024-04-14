using _Project.StatSystem.Common;
using UnityEngine;
using Zenject;

namespace _Project.StatSystem
{
    public class StatService : MonoInstaller
    {
        [SerializeField] private StatManager _statManager;

        public override void InstallBindings()
        {
            Container.Bind<IStatProvider>().FromInstance(_statManager).AsSingle();
        }
    }
}