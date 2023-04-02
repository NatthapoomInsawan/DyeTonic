using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class GameProgressUpdater : MonoBehaviour
    {
        [Header("Scriptable Object referencing")]
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private SongManager _songManager;

        [Header("Story Mode song list")]
        [SerializeField] private List<SongData> _songList;

        // Start is called before the first frame update
        void Start()
        {
            if (_songManager.IsStoryMode() && _songManager.HP != 0)
                UpdateStoryProgress();
            else
                gameObject.SetActive(false);
        }

        private void UpdateStoryProgress()
        {
            if (_gameSettings.progress >= _songList.Count)
                return;

            if (_songManager.GetCurrentSongData() == _songList[_gameSettings.progress])
            {
                _gameSettings.progress++;

                _gameSettings.SaveData();
            }
        }

        public void OnStageClear()
        {
            if (_songManager.IsStoryMode() && _gameSettings.progress == 3 && _songManager.HP != 0)
                SceneManager.LoadScene("Chapter 3_2");
            else
                SceneLoader.LoadPreviousScene();
        }
    }
}
