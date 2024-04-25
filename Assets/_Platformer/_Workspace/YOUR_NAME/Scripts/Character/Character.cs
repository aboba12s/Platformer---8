using System;
using UnityEngine;

namespace YOUR_NAME
{
    public class Character : MonoBehaviour, IDamageble
    {
        [SerializeField] private Movement movement;
        [SerializeField] private Interactor interactor;
        [SerializeField] private MeleeWeapon weapon;
        [Space]
        [SerializeField] private Health Health;
        [SerializeField] private DamageCapDecorator damageCap;

        private IHealth _health;

        private InputManager _inputManager;
        
        public Movement Movement => movement;
        
        #region Input Setup
        
        public InputManager InputManager
        {
            get => _inputManager;
            set
            {
                RemovePlayerInput(_inputManager);
                _inputManager = value;
                SetupPlayerInput(_inputManager);
            }
        }

        private void SetupPlayerInput(InputManager inputManager)
        {
            if (inputManager == null) return;
            
            RemovePlayerInput(inputManager);
            
            inputManager.onJump += OnJump;
            inputManager.onInteract += OnInteract;
            inputManager.onAttack += OnAttack;
        }

        private void RemovePlayerInput(InputManager inputManager)
        {
            if (inputManager == null) return;
            
            inputManager.onJump -= OnJump;
            inputManager.onInteract -= OnInteract;
            inputManager.onAttack -= OnAttack;
        }

        private void OnAttack()
        {
            weapon.Attack();
        }

        private void InitInputManager()
        {
            if (InputManager) return;
            
            var go = new GameObject("InputManager");
            go.transform.SetParent(gameObject.transform);
            InputManager = go.AddComponent<InputManager>();
        }
        
        #endregion

        #region Unity Events

        private void Awake()
        {
            InitInputManager();
        }

        private void OnEnable()
        {
            SetupPlayerInput(_inputManager);
        }
        
        private void OnDisable()
        {
            RemovePlayerInput(_inputManager);
        }

        private void FixedUpdate()
        {
            if (!_inputManager) return;
            OnMove(_inputManager.Move);
        }

        #endregion

        private void Start()
        {
            _health = damageCap.Assign(Health);
        }

        private void OnMove(Vector2 velocity)
        {
            movement.Move(velocity);
        }
        
        private void OnJump()
        { 
            movement.DoJump();
        }

        private void OnInteract()
        {
            interactor.Interact();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
        }
    }
}