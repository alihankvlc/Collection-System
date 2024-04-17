using System;
using System.Numerics;
using _Project.InventorySystem.Common;
using _Project.Player.InputSystem;
using _Project.StatSystem.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Player.Common.ThirdPerson.Controller
{
    public enum CursorLockState
    {
        Locked,
        Unlocked
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float _runSpeed = 3f;

        [SerializeField] private float _sprintSpeed = 6f;
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private float _rotationSpeed = 15f;
        [SerializeField] private float _rotationSmoothDampTime = 0.12f;
        [SerializeField] private float _speedChangeRate = 10f;
        [SerializeField, ReadOnly] private float _targetSpeed;


        [Header("Stat Settings")] [SerializeField]
        private int _sprintStaminaDecreaseAmount;

        [SerializeField] private int _staminaIncreaseAmount;
        [SerializeField, ReadOnly] private bool _canSprint;

        [Header("Ground Settings")] [SerializeField]
        private LayerMask _groundLayerMask;

        [SerializeField] private float _groundRadius;
        [SerializeField] private float _groundOffset;
        [SerializeField, ReadOnly] private bool _isGrounded;

        [Header("Camera Settings")] [SerializeField]
        private GameObject _cameraTargetObject;

        [SerializeField, Range(-360, 360)] private int _cameraBottomClamp = -60;
        [SerializeField, Range(-360, 360)] private int _cameraTopClamp = 70;

        [Header("Cursor Settings")] [SerializeField]
        private bool _cursorLock;

        [SerializeField] private CursorLockMode _cursorLockMode;

        [Header("Animatio Rigging Settings")] [SerializeField]
        private GameObject _lookAtTargetObject;

        [SerializeField] private float _lookingMultiplier;


        private float _targetRotation;
        private float _verticalVelocity;
        private float _cameraTargetPitch;
        private float _cameraTargetYaw;
        private float _currentVelocityForRotation;
        private float _animationBlend;

        private const float _gravity = -15.0f;
        private const float _threshold = 0.1f;

        private Animator _animator;
        private CharacterController _controller;
        private Camera _mainCamera;

        [Inject] private InputHandler _input;
        [Inject] private IStatProvider _statProvider;

        private readonly int _horizontalVelocityHashId = Animator.StringToHash("HorizontalVelocity");
        private readonly int _verticalVelocityHashId = Animator.StringToHash("VerticalVelocity");
        private readonly int _speedHashId = Animator.StringToHash("Speed");
        private readonly int _jumpHashId = Animator.StringToHash("Jump");
        private readonly int _walkHashId = Animator.StringToHash("Walk");
        private readonly int _groundedHashId = Animator.StringToHash("Grounded");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;

            Cursor.visible = !_cursorLock;
            Cursor.lockState = _cursorLockMode == CursorLockMode.Locked
                ? CursorLockMode.Locked
                : CursorLockMode.None;
        }

        private void Update()
        {
            if (InventoryManager.Instance.IsOpen)
            {
                _targetSpeed = 0.0f;
                _animator.SetFloat(_speedHashId, _targetSpeed);
                return;
            }

            Movement();
            GroundedCheck();

            if (_input.Jump) Jump();
        }

        private void FixedUpdate()
        {
            if (InventoryManager.Instance.IsOpen)
            {
                _targetSpeed = 0.0f;
                _animator.SetFloat(_speedHashId, _targetSpeed);
                return;
            }

            PlayerRigLayerController();
        }

        private void LateUpdate()
        {
            if (InventoryManager.Instance.IsOpen)
            {
                _targetSpeed = 0.0f;
                _animator.SetFloat(_speedHashId, _targetSpeed);
                return;
            }

            CameraRotation();
        }

        private void PlayerRigLayerController()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 lookAtTargetPosition = _lookAtTargetObject.transform.position;
                Vector3 moveToLookAtPosition = Vector3.Lerp(lookAtTargetPosition, hitInfo.point,
                    Time.deltaTime * _lookingMultiplier);

                _lookAtTargetObject.transform.position = moveToLookAtPosition;
            }
        }

        private void CameraRotation()
        {
            if (_input.Look.sqrMagnitude > _threshold)
            {
                float deltaMultiplier = 1.0f;

                _cameraTargetPitch += _input.Look.y * deltaMultiplier;
                _cameraTargetYaw += _input.Look.x * deltaMultiplier;
            }

            _cameraTargetPitch = Mathf.Clamp(_cameraTargetPitch, _cameraBottomClamp, _cameraTopClamp);
            _cameraTargetObject.transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0.0f);
        }

        private void Movement()
        {
            Vector3 moveDirection = new(_input.Move.x, 0.0f, _input.Move.y);

            if (_input.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                float rotation = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _targetRotation,
                    ref _currentVelocityForRotation, _rotationSmoothDampTime);

                Quaternion lookAtRotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, lookAtRotation, _rotationSpeed);
            }

            Vector3 targetDirection =
                Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            targetDirection.Normalize();

            _canSprint = _statProvider.GetStat<Stamina>(1).CurrentValue > 0;

            _statProvider.GetStat<Stamina>(1).Modify +=
                _input.Sprint
                    ? -_sprintStaminaDecreaseAmount * Time.deltaTime
                    : _staminaIncreaseAmount * Time.deltaTime;

            _targetSpeed = _input.Move != Vector2.zero
                ? (_input.Sprint && _canSprint ? _sprintSpeed : _runSpeed)
                : 0;

            _animationBlend = Mathf.MoveTowards(_animationBlend,
                _targetSpeed * Mathf.Clamp01(_controller.velocity.magnitude / _runSpeed),
                Time.deltaTime * _speedChangeRate);


            _controller.Move(targetDirection * (_targetSpeed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _animator.SetFloat(_speedHashId, _animationBlend);
        }

        private void Jump()
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
            _animator.Play(_jumpHashId);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundOffset,
                transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, _groundRadius, _groundLayerMask,
                QueryTriggerInteraction.Ignore);

            Gravity();

            _animator.SetBool(_groundedHashId, _isGrounded);
        }

        private void Gravity()
        {
            if (_isGrounded && _verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            _verticalVelocity += _gravity * Time.deltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundOffset,
                transform.position.z);

            Gizmos.color = _isGrounded ? Color.red : Color.green;
            Gizmos.DrawWireSphere(spherePosition, _groundRadius);
        }
    }
}