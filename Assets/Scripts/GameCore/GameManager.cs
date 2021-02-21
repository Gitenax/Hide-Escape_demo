using System;
using EnemyCore;
using PlayerCore;
using UI;
using UnityEngine;


namespace GameCore
{
    /// <summary>
    /// Агрегатор классов для управления игровым процессом
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
#pragma warning disable CS0649
        [Header("Игрок"), SerializeField] 
        private Player _player;

        [Header("Противники"), SerializeField]
        private Enemy[] _enemies; // Полицейские

        [Header("Игровые окна")] [SerializeField]
        private MenuBase[] _menus;
        
        
        [SerializeField] private EndGameMenu _endGameMenu;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private CompleteLevelMenu _completeLevelMenu;
#pragma warning restore CS0649
        
        private Level _level;
        
        // Кривая реализация синглтона которая по сути и не нужна толком
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        var obj = new GameObject("~GameManager");
                        _instance = obj.AddComponent<GameManager>();
                        DontDestroyOnLoad(obj);
                    }
                }

                return _instance;
            }
        }


        // Агрегирование класса Level для управления переключений между сценами
        public void LoadNextLevel() => _level.Next();
        public void LoadMainMenu() => _level.Load(0);
        public void LoadFirstLevel() => _level.Load(1);
        public void RestartLevel() => _level.Restart();
        
        public void LevelComplete()
        {
            _completeLevelMenu.Show();
            _player.PlayerInput.Disable();
        }
        
        public void FinishGame()
        {
            _endGameMenu.Show();
        }
        
        
        private void Awake()
        {
            _level = new Level();

            _menus = FindObjectsOfType<MenuBase>(true);
            
            // Подписка на событие смерти игрока(от лазерной сетки)
            if(_player != null)
                _player.PlayerDied += OnPlayerDied;

            // Подписка на событие обнаружение полицаями игрока
            if (_enemies == null) return;
            
            foreach (var enemy in _enemies)
                enemy.GetComponent<FieldOfView>().PlayerDetected += OnPlayerDetected;
        }

        private void Start()
        {
            InitializeWindows();
        }

        private void InitializeWindows()
        {
            foreach (var menuBase in _menus)
            {
                if (menuBase is MainMenu menuMain)
                {
                    menuMain.StartGameButton.ButtonClick = LoadFirstLevel;
                    menuMain.QuitButton.ButtonClick = Quit;
                    continue;
                }

                if (menuBase is EndGameMenu menuEnd)
                {
                    if(!menuEnd.gameObject.activeSelf)
                        menuEnd.gameObject.SetActive(true);
                    
                    menuEnd.RestartButton.ButtonClick = RestartLevel;
                    menuEnd.QuitButton.ButtonClick = Quit;
                    menuEnd.gameObject.SetActive(false);
                    continue;
                }

                if (menuBase is CompleteLevelMenu menuComplete)
                {
                    if(!menuComplete.gameObject.activeSelf)
                        menuComplete.gameObject.SetActive(true);
                
                    menuComplete.ContinueButton.ButtonClick = LoadNextLevel;
                    menuComplete.RestartButton.ButtonClick = RestartLevel;
                    menuComplete.QuitButton.ButtonClick = Quit;
                    menuComplete.gameObject.SetActive(false);
                    continue;
                }
                
                if (menuBase is GameFinishMenu menuFinish)
                {
                    if(!menuFinish.gameObject.activeSelf)
                        menuFinish.gameObject.SetActive(true);
                
                    menuFinish.MenuButton.ButtonClick = LoadMainMenu;
                    menuFinish.RestartButton.ButtonClick = RestartLevel;
                    menuFinish.QuitButton.ButtonClick = Quit;
                    menuFinish.gameObject.SetActive(false);
                }
            }
        }
        
        public void Quit() => Application.Quit();
        
        private void OnPlayerDied(Player player)
        {
            FinishGame();
            Debug.Log("<b><color=red>ВНИМАНИЕ! Игрок убит!</color></b>");
        }
    
        private void OnPlayerDetected()
        {
            FinishGame();
            _player.PlayerInput.Disable();
            Debug.Log("<b><color=red>ВНИМАНИЕ! Игрок замечен!</color></b>");
        }
    }
}
