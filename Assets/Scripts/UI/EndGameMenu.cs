using System;
using UnityEngine;

namespace UI
{
    public sealed class EndGameMenu : MenuBase
    {
        public MenuButton RestartButton { get; private set;  }
        public MenuButton QuitButton { get; private set;  }
        
        
        protected override void SetButtons()
        {
            RestartButton = GameObject.Find("EndGameMenu_RestartButton").GetComponent<MenuButton>();
            QuitButton = GameObject.Find("EndGameMenu_QuitButton").GetComponent<MenuButton>();
        }
    }
}
