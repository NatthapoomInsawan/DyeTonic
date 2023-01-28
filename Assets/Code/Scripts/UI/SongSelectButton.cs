using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public class SongSelectButton : MonoBehaviour
    {
        [SerializeField] private SongData _songData;
        [SerializeField] private TextMeshProUGUI songNameText;
        [SerializeField] private TextMeshProUGUI artistNameText;
        [SerializeField] private Image songImage;

        // Start is called before the first frame update
        void Start()
        {
            //song name text
            if (_songData.songName != null)
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

        }

        public void OnButtonClick()
        {

        }

    }
}
