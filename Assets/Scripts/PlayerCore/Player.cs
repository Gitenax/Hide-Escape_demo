using System;
using UnityEngine;
using Input;


namespace PlayerCore
{
    /// <summary>
    /// Представление игрока
    /// </summary>
    public class Player : MonoBehaviour
    {    
#pragma warning disable CS0649
        [SerializeField] private int _health = 100;
        [SerializeField] private float _moveSpeed = 50f;
        [SerializeField] private Vector2 _moveDirection;
#pragma warning restore CS0649     
        
        private CharacterController _characterController;
        private InputActions _input; // New Input system
        
        public event Action<Player, int> HealthChanged;
        public event Action<Player> PlayerDied;
        

        public int Health
        {
            get => _health;
            set
            {
                if (_health != value)
                {
                    _health = value;
                    if (_health < 0)
                    {
                        _health = 0;
                        OnDied();
                    }
                    
                    HealthChanged?.Invoke(this, Health);
                }
            }
        }

        /// <summary>
        /// Управление игрока с Input System
        /// </summary>
        public InputActions.PlayerActions PlayerInput => _input.Player;
        
        
        public void TakeDamage(int amount) => Health -= amount;


        private void Awake()
        {
            _input = new InputActions();
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable() => PlayerInput.Enable();

        private void OnDisable() => PlayerInput.Disable();

        private void Update() => Move();
        
        private void Move()
        {
            _moveDirection = PlayerInput.Move.ReadValue<Vector2>();
            var dir = new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed;
            _characterController.Move(dir * Time.deltaTime);
            
            // Нужно для фиксации положения игрока по Y на одной высоте, т.к. был бак с дверьми когда они приподнимали игрока при закрытии
            var pos = transform.position;
            transform.position = new Vector3(pos.x, 1f, pos.z);
        }

        private void OnDied()
        {
            PlayerInput.Disable();
            PlayerDied?.Invoke(this);
        }
    }
}