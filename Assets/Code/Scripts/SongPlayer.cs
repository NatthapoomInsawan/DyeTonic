using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{

    public class SongPlayer : MonoBehaviour
    {
        [Header("Scriptable Objects referencing")]
        [SerializeField] SongManager _songManager;
        [SerializeField] SongData _songData;
        [SerializeField] AudioChannelSO _channelSO;

        //the duration of a beat
        [SerializeField] private float secPerBeat;

        //the current position of the song (in seconds)
        [SerializeField] private float songPosition;

        //Song position in beat to show
        [SerializeField] private float songPositionInBeats;

        private bool songEnd;
        private bool gameLose;

        public static event Action OnSongeEnd;
        public static event Action<bool> OnGameEnd;

        //how much time (in seconds) has passed since the song started
        private float dspSongTime;

        private void Awake()
        {
            //if songdata is null load current songdata from songManager
            if (_songData == null)
                _songData = _songManager.GetCurrentSongData();

            //reset songPositionInBeats to 0
            _songManager.songPosInBeats = 0;

            //reset song combo
            _songManager.songCombo = 0;

            //reset score multiplier
            _songManager.scoreMultiplier = 1;

            //reset score
            _songManager.player1Score = 0;
            _songManager.player2Score = 0;

            //reset player hit notes
            for (int i = 0; i < 4; i++)
            {
                _songManager.play1HitNotes[i] = 0;
                _songManager.play2HitNotes[i] = 0;
            }

            //reset song HP
            _songManager.HP = 100;
        }

        // Start is called before the first frame update
        private void Start()
        {
            //calculate how many seconds is one beat
            secPerBeat = 60f / _songData.songBpm;

            //record the time when the song starts
            dspSongTime = (float)AudioSettings.dspTime;

            //start the song
            _channelSO.RaisePlayRequest(_songData.song);

        }

        // Update is called once per frame
        private void Update()
        {
            //calculate the position in seconds
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);

            //calculate the position in beats
            _songManager.songPosInBeats = songPosition / secPerBeat;

            songPositionInBeats = _songManager.songPosInBeats;

            //invoke song end
            if (songPosition >= _songManager.GetCurrentSongData().song.length && songEnd != true)
            {
                OnSongeEnd?.Invoke();
                songEnd = true;
            }

            if (_songManager.HP <= 0)
                gameLose = true;

            //game over condition
            if (songPosition >= _songManager.GetCurrentSongData().song.length + 2 || gameLose)
                GameOver();

        }

        private void GameOver()
        {
            DOTween.Clear();
            if (!PhotonNetwork.InRoom)
            {
                if (gameLose)
                    _songManager.HP = 0;
                SceneManager.LoadScene("GameOverScene", LoadSceneMode.Additive);
            }
            else
                OnGameEnd?.Invoke(gameLose);
            gameObject.SetActive(false);
        }

    }
}
