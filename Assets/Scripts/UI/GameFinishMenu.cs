using UnityEngine;

namespace UI
{
    public sealed class GameFinishMenu : MenuBase
    {
        public MenuButton MenuButton { get; private set;  }
        public MenuButton RestartButton { get; private set;  }
        public MenuButton QuitButton { get; private set;  }
        

        protected override void SetButtons()
        {
            MenuButton = GameObject.Find("GameFinishMenu_MainMenu").GetComponent<MenuButton>();
            RestartButton = GameObject.Find("GameFinishMenu_RestartButton").GetComponent<MenuButton>();
            QuitButton = GameObject.Find("GameFinishMenu_QuitButton").GetComponent<MenuButton>();
        }
    }
}
