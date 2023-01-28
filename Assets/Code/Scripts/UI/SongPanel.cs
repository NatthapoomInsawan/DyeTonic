using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public class SongPanel : SongCoverDisplay
    {
        private void OnEnable()
        {
            SongSelectButton.OnSongSelectButtonClick += UpdateSongPanel;
        }

        private void OnDisable()
        {
            SongSelectButton.OnSongSelectButtonClick -= UpdateSongPanel;
        }


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        private void UpdateSongPanel(SongData songData)
        {
            _songData = songData;

            Start();
        }
    }
}
