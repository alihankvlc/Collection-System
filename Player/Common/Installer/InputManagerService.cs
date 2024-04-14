using _Project.Player.InputSystem;
using UnityEngine;
using Zenject;

namespace _Project.Common
{
    public class InputManagerService : MonoInstaller
    {
        [SerializeField] private InputManager _inputManager;

        public override void InstallBindings()
        {

            Container.Bind<InputHandler>().To<InputManager>().FromInstance(_inputManager).AsSingle();
        }
    }
}