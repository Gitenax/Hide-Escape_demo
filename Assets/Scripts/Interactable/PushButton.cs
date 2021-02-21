using System;
using PlayerCore;
using UnityEngine;

namespace Interactable
{
    /// <summary>
    /// Обычная нажимная кнопка
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class PushButton : InteractableObject
    {
#pragma warning disable CS0649
        [SerializeField] private bool _isToggle;

        [Header("Меш кнопки")]
        [SerializeField] private Transform _buttonTransform;
        [SerializeField] private MeshRenderer _buttonRenderer;
        [SerializeField] private Light _light;
#pragma warning restore CS0649   
        
        private Color _defaultColor;
        private AudioSource _audio;

   
        public override void Interact()
        {
            State = !State;

            var pos = _buttonTransform.localPosition;
            
            // Смена состояния кнопки после взаимодействия
            if (State)
            {
                _buttonTransform.localPosition = new Vector3(pos.x, -0.08f, pos.z);
                _buttonRenderer.material.color = Color.green;
                _light.color = Color.green;
            }
            else
            {
                _buttonTransform.localPosition = new Vector3(pos.x, 0, pos.z);
                _buttonRenderer.material.color = _defaultColor;
                _light.color = _defaultColor;
            }
            _audio.Play();
        }

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _defaultColor = _buttonRenderer.material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                // Реализация возможности только один раз взаимодействовать с кнопкой
                // p.s. но похоже без флага isToggle работает через жопу
                if (_disactivate)
                {
                    if(_activated == false)
                    {
                        Interact();
                        _activated = true;
                    }
                }
                else
                    Interact();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_isToggle)
                return;
            
            Interact();
        }
    }
}
