using System;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Камера следующая за целью
    /// </summary>
    public class TrackingCamera : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Transform _target;
        [SerializeField] private float _cameraLerp = 5f;
#pragma warning restore CS0649
        
        // Смещение от цели
        private Vector3 _cameraOffset;
        

        private void Start()
        {
            _cameraOffset = new Vector3(0, 10, -6f);
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, _target.position + _cameraOffset, _cameraLerp * Time.deltaTime);
        }
    }
}
