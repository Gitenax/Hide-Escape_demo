using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Базовый класс представления панелей меню
    /// </summary>
    public abstract class MenuBase : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            SetButtons();
        }

        protected abstract void SetButtons();
    }
}
