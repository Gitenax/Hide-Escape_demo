using System;
using UnityEngine;

namespace Interactable
{
    /// <summary>
    /// Представление базового класса "взаимодействующих" объектов
    /// </summary>
    public abstract class InteractableObject : MonoBehaviour
    {
        [Header("Состояние (вкл/выкл)"), SerializeField] 
        private bool _state;
        
        [Tooltip("Отключить переключение поселе успешного взаимодействия"), SerializeField] 
        protected bool _disactivate;
        protected bool _activated;
        
        [Header("Внешний переключатель"), SerializeField] 
        private InteractableObject _externalSource;
        
        public event Action<bool> StateChanged;
        public event Action StateOn;
        public event Action StateOff;
        
        public bool State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    StateChanged?.Invoke(_state);
                    
                    if(_state)
                        StateOn?.Invoke();
                    else
                        StateOff?.Invoke();
                }
            }
        }

        public InteractableObject ExternalSource
        {
            get => _externalSource;
            set => _externalSource = value;
        }
        
        public abstract void Interact();
    }
}
