using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class MainMenu : MenuBase
    {
        public MenuButton StartGameButton { get; private set;  }
        public MenuButton QuitButton { get; private set;  }
        
        
        protected override void SetButtons()
        {
            StartGameButton = GameObject.Find("MainMenu_StartGameButton").GetComponent<MenuButton>();
            QuitButton = GameObject.Find("MainMenu_QuitButton").GetComponent<MenuButton>();
        }
    }
}
