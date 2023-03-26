using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (_songManager.IsStoryMode())
                UpdateStoryProgress();
            else
                gameObject.SetActive(false);
        }

        private void UpdateStoryProgress()
        {
            if (_songManager.GetCurrentSongData() == _songList[_gameSettings.progress])
            {
                _gameSettings.progress++;
                _gameSettings.SaveData();
            }
        }
    }
}
