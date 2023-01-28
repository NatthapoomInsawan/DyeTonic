using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    [RequireComponent(typeof(AudioSource))]
    public class SongPanel : SongCoverDisplay
    {

        [SerializeField] private AudioSource audioSource;

        private void OnEnable()
        {
            SongSelectButton.OnSongSelectButtonClick += UpdateSongCoverDisplay;
        }

        private void OnDisable()
        {
            SongSelectButton.OnSongSelectButtonClick -= UpdateSongCoverDisplay;
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void UpdateSongCoverDisplay(SongData songData)
        {
            base.UpdateSongCoverDisplay(songData);

            //play song
            if (songData.song != null)
            {
                audioSource.clip = songData.song;
                //play faster at 9.5 sec
                audioSource.time = 9.5f;
                audioSource.Play();
            }
        }
    }
}
