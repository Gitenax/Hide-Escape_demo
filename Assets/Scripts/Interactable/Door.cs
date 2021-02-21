using System;
using System.Collections;
using PlayerCore;
using UnityEngine;

namespace Interactable
{
    /// <summary>
    /// Представление двери
    /// </summary>
    public class Door : InteractableObject
    {
#pragma warning disable CS0649
        [SerializeField] private Animator _doorAnimator;
#pragma warning restore CS0649
        
        private MeshRenderer[] _meshRenderers;
        
        public override void Interact()
        {
            //Todo: do nothing...
        }


        private void Start()
        {
            _meshRenderers = GetComponentsInChildren<MeshRenderer>();
            
            // Подписка на внешний триггер
            if (ExternalSource != null)
                ExternalSource.StateChanged += ExternalSourceOnStateChanged;
        }

        private void ExternalSourceOnStateChanged(bool state)
        {
            
            if (state)
            {
                foreach (var renderer in _meshRenderers)
                    renderer.material.color = Color.green;
            }
            else
            {
                foreach (var renderer in _meshRenderers)
                    renderer.material.color = Color.red;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                // Если нет внешнего триггера, то активация по собственному колайдеру
                if (ExternalSource == null)
                    OpenDoor();
                else
                {
                    // Активация в зависимости от состоянию вшеншего триггера
                    if (ExternalSource.State)
                        OpenDoor();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                if (ExternalSource == null)
                    CloseDoor();
                else
                {
                    if (ExternalSource.State)
                        CloseDoor();
                }
            }
        }

        // Анимации отытия
        private void OpenDoor()
        {
            _doorAnimator.SetBool("DoorIdleClose", false);
            _doorAnimator.SetBool("DoorOpen", true);
            _doorAnimator.SetBool("DoorClose", false);
        }

        // Анимации закрытия
        private void CloseDoor()
        {
            _doorAnimator.SetBool("DoorOpen", false);
            _doorAnimator.SetBool("DoorClose", true);
            _doorAnimator.SetBool("DoorIdleClose", true);
        }
    }
}
