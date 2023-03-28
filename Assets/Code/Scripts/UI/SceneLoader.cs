using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class SceneLoader : MonoBehaviour
    {

        public static string PreviousScene { get; private set; }

        public void LoadScene(string sceneName)
        {
            PreviousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void UnLoadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public static void LoadPreviousScene()
        {
            if (PreviousScene != null)
                SceneManager.LoadSceneAsync(PreviousScene);
            else
                Debug.LogWarning("PreviousScene is null");
        }

    }
}
