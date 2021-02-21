using System;
using GameCore;
using PlayerCore;
using UnityEngine;
using UnityEngine.Events;

namespace Interactable
{
    /// <summary>
    /// Тригер завершения уровня
    /// </summary>
    public class LevelTrigger : InteractableObject
    {
#pragma warning disable CS0649
        [SerializeField] private BoxCollider _collider;
#pragma warning restore CS0649
        
        private AudioSource _audio;
        
        public override void Interact()
        {
            _audio.Play();
            GameManager.Instance.LevelComplete();
        }


        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<Player>(out _))
                Interact();
        }


#if UNITY_EDITOR
        // Отрисовка куба по размеру коллайдера в редакторе, т.к. это просто пустой объект с колайдером
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, _collider.size);
        }
#endif
    }
}
