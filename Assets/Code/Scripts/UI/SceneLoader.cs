using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class SceneLoader : MonoBehaviour
    {

        public static string PreviousScene { get; private set; }

        private void OnDestroy()
        {
            PreviousScene = SceneManager.GetActiveScene().name;
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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
