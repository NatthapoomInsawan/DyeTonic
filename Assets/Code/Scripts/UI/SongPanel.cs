using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public class SongPanel : SongCoverDisplay
    {
        
        [SerializeField] private SongManager _songManager;
        [SerializeField] private AudioChannelSO _channelSO;

        private void OnEnable()
        {
            SongSelectButton.OnSongSelectButtonClick += UpdateSongCoverDisplay;
        }

        private void OnDisable()
        {
            SongSelectButton.OnSongSelectButtonClick -= UpdateSongCoverDisplay;
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

            //set current songData
            _songManager.SetSongData(songData);

            //play song
            if (songData.song != null)
            {
                _channelSO.RaisePlayRequest(songData.song, 9.5f);
            }
        }
    }
}
