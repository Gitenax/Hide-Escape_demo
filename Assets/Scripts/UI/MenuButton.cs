using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        private Button _button;

        public UnityAction ButtonClick
        {
            set
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                    
                _button.onClick.AddListener(value);
                Debug.Log($"Задано событие для кнопки <b><color=orange>{_button.name}</color></b> : <i><color=green>{value.Method.Name}</color></i>");
            }
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
    }
}
