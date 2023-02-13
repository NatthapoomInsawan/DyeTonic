using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class RoomSelectButton : SongCoverDisplay
    {
        [SerializeField] private TextMeshProUGUI roomNameText;
        public string RoomName { get; set; }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public void OnButtonClick()
        {
            SceneManager.LoadScene("RoomScene");
        }

        public override void UpdateSongCoverDisplay(SongData songData)
        {
            _songData = songData;

            //song name text
            if (_songData.songName != null || _songData.songName == "")
                songNameText.text = _songData.songName;
            else
                songNameText.text = _songData.name;

            //artist text
            if (_songData.songArtist != null)
                artistNameText.text = _songData.songArtist;
            else
                artistNameText.text = "";

            //song Image
            if (songImage != null)
                songImage.sprite = _songData.songSprite;

            //set room name
            roomNameText.text = RoomName;

        }

    }
}
