using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public sealed class Level
    {
        public void Next()
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if(SceneManager.sceneCount > nextScene)
                SceneManager.LoadScene(nextScene);
            else
                Load(0);
        }

        public void Load(string sceneName)
        {
            Load(SceneManager.GetSceneByName(sceneName).buildIndex);
        }
        
        public void Load(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }
        
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
