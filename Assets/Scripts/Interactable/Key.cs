using PlayerCore;
using UnityEngine;

namespace Interactable
{
    /// <summary>
    /// Представление "ключа"
    /// </summary>
    public class Key : InteractableObject
    {
#pragma warning disable CS0649
        [SerializeField] private float _rotateSpeed = 15f;
        [SerializeField] private Transform[] _children;
#pragma warning restore CS0649
        
        private AudioSource _audio;
        private CapsuleCollider _collider;
        
        public override void Interact()
        {
            State = true;
        }

        
        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _collider = GetComponent<CapsuleCollider>();
        }
        

        private void Update()
        {
            // Вращение вокруг своей оси
            transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out _))
            {
                Interact();
                _collider.enabled = false;
                _audio.Play();
            
                // Скрыть видимые части для воспроиизведения звука до уничтожения
                foreach (var child in _children)
                    child.gameObject.SetActive(false);
            
                Destroy(gameObject, _audio.clip.length);
            }
        }
    }
}
