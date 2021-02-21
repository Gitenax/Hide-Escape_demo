using UnityEngine;

namespace UI
{
    public sealed class CompleteLevelMenu : MenuBase
    {
        public MenuButton ContinueButton { get; private set;  }
        public MenuButton RestartButton { get; private set;  }
        public MenuButton QuitButton { get; private set;  }
        

        protected override void SetButtons()
        {
            ContinueButton = GameObject.Find("ContinueLevelMenu_NextLevelButton").GetComponent<MenuButton>();
            RestartButton = GameObject.Find("ContinueLevelMenu_RestartButton").GetComponent<MenuButton>();
            QuitButton = GameObject.Find("ContinueLevelMenu_QuitButton").GetComponent<MenuButton>();
        }
    }
}
