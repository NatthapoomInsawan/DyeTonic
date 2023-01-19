using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DyeTonic
{
    public class LevelEditorSongPlayer : MonoBehaviour
    {
        [Header("Scriptable Objects referencing")]
        [SerializeField] SongManager _songManager;
        [SerializeField] SongData _songData;

        [Header("UI Objects referencing")]
        [SerializeField] TextMeshProUGUI _textBox;
        [SerializeField] Slider _timelineSlider;
        [SerializeField] TextMeshProUGUI _playButtonText;

        [Header("Song Information")]
        //the duration of a beat
        [SerializeField] float secPerBeat;

        //the current position of the song (in seconds)
        [SerializeField] float songPosition;

        //Song position in beat to show
        [SerializeField] float songPositionInBeats;

        float oldSongPosition;
        float changeValue;

        bool songPlay = false;

        AudioSource audioSource;

        //how much time (in seconds) has passed since the song started
        float dspSongTime;

        private void Awake()
        {
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

            //set up AudioSource
            audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //record the time when the song starts
            dspSongTime = (float)AudioSettings.dspTime;

            //load song
            LoadSong();

            //play song
            PlaySong();

        }

        // Update is called once per frame
        void Update()
        {
            if (songPlay)
            {
                //calculate the position in seconds
                songPosition = (float)(AudioSettings.dspTime - dspSongTime);

                //calculate the position in beats
                _songManager.songPosInBeats = songPosition / secPerBeat;

                songPositionInBeats = _songManager.songPosInBeats;

                //update timeline slider
                _timelineSlider.value = songPosition;

                //update old song position
                oldSongPosition = songPosition;

            }
            else
                dspSongTime = (float)AudioSettings.dspTime;

            UpdateUI();

        }

        void LoadSong()
        {
            //calculate how many seconds is one beat
            secPerBeat = 60f / _songData.songBpm;

            audioSource.clip = _songData.song;

            _timelineSlider.maxValue = audioSource.clip.length;
        }

        private void PlaySong()
        {

            audioSource.Play();
            songPlay = true;
        }

        private void UnPauseSong()
        {
            dspSongTime = (float)AudioSettings.dspTime - oldSongPosition;

            if (changeValue != 0)
            {
                dspSongTime -= changeValue;
                audioSource.time = songPosition;
                audioSource.Play();
            }
            else
                audioSource.UnPause();

            songPlay = true;
            changeValue = 0;
        }

        private void PauseSong()
        {
            audioSource.Pause();
            songPlay = false;
        }

        void UpdateUI()
        {
            _textBox.text = "Song name: " +_songData.name + "\n" +
                           "BPM: " + _songData.songBpm + "\n" +
                           "Sec per beat: " + secPerBeat + "\n" +
                           "Song position in sec: " + songPosition + "\n" +
                           "Song position in beat: " + songPositionInBeats;

        }

        public void OnPlayButton()
        {
            if (songPlay)
            {
                _playButtonText.text = "Play";
                PauseSong();
            }
            else 
            {
                _playButtonText.text = "Pause";
                UnPauseSong();
            }

        }

        public void OnTimelineSliderChange()
        {

            if (!songPlay)
            {
                songPosition = _timelineSlider.value;

                ////calculate the position in beats
                _songManager.songPosInBeats = songPosition / secPerBeat;

                songPositionInBeats = _songManager.songPosInBeats;

                dspSongTime = (float)AudioSettings.dspTime - oldSongPosition;

                songPosition = (float)AudioSettings.dspTime - dspSongTime;

                changeValue = _timelineSlider.value - songPosition;

                songPosition = _timelineSlider.value;

            }

            //debug.log("dsp time:" + audiosettings.dsptime);
            //debug.log("timer stamp:" + dspsongtime);
            //debug.log("difference:" + (audiosettings.dsptime - dspsongtime));
        }

    }
}
