using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Player.InputSystem
{
    public interface InputHandler
    {
        public Vector2 Move { get; }
        public Vector3 Look { get; }
        public bool Sprint { get; }
        public bool Walk { get; }
        public bool Jump { get; }
    }

    public class InputManager : MonoBehaviour, InputHandler
    {
        [SerializeField] private PlayerInput _input;

        private InputActionMap _actionMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _sprintAction;
        private InputAction _walkInputAction;
        private InputAction _jumpInputAction;

        private const string _inputMoveEntry = "Move";
        private const string _inputLookEntry = "Look";
        private const string _inputSprintEntry = "Sprint";
        private const string _inputWalkEntry = "Walk";
        private const string _inputJumpEntry = "Jump";
        public Vector2 Move { get; private set; }
        public Vector3 Look { get; private set; }

        public bool Sprint { get; private set; }
        public bool Walk { get; private set; }
        public bool Jump { get; private set; }

        private void Awake()
        {
            _actionMap = _input.currentActionMap;
            _moveAction = _actionMap.FindAction(_inputMoveEntry);
            _lookAction = _actionMap.FindAction(_inputLookEntry);
            _sprintAction = _actionMap.FindAction(_inputSprintEntry);
            _walkInputAction = _actionMap.FindAction(_inputWalkEntry);
            _jumpInputAction = _actionMap.FindAction(_inputJumpEntry);
        }

        private void OnEnable()
        {
            _actionMap.Enable();
        }

        private void Start()
        {
            _moveAction.performed += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveAction.canceled += (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

            _lookAction.performed += (InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();
            _lookAction.canceled += (InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();

            _sprintAction.performed += (InputAction.CallbackContext context) => Sprint = context.ReadValueAsButton();
            _sprintAction.canceled += (InputAction.CallbackContext context) => Sprint = context.ReadValueAsButton();

            _walkInputAction.performed += (InputAction.CallbackContext context) => Walk = context.ReadValueAsButton();
            _walkInputAction.canceled += (InputAction.CallbackContext context) => Walk = context.ReadValueAsButton();

            _jumpInputAction.started += (InputAction.CallbackContext context) => Jump = context.ReadValueAsButton();
            _jumpInputAction.canceled += (InputAction.CallbackContext context) => Jump = context.ReadValueAsButton();
        }

        private void OnDisable()
        {
            _moveAction.performed -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
            _moveAction.canceled -= (InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();

            _lookAction.performed -= (InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();
            _lookAction.canceled -= (InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();

            _sprintAction.performed -= (InputAction.CallbackContext context) => Sprint = context.ReadValueAsButton();
            _sprintAction.canceled -= (InputAction.CallbackContext context) => Sprint = context.ReadValueAsButton();

            _walkInputAction.performed -= (InputAction.CallbackContext context) => Walk = context.ReadValueAsButton();
            _walkInputAction.canceled -= (InputAction.CallbackContext context) => Walk = context.ReadValueAsButton();

            _jumpInputAction.started -= (InputAction.CallbackContext context) => Jump = context.ReadValueAsButton();
            _jumpInputAction.canceled -= (InputAction.CallbackContext context) => Jump = context.ReadValueAsButton();

            _actionMap.Disable();
        }
    }
}