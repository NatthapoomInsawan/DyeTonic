using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public abstract class SongCoverDisplay : MonoBehaviour
    {
        public SongData _songData;
        [SerializeField] protected TextMeshProUGUI songNameText;
        [SerializeField] protected TextMeshProUGUI artistNameText;
        [SerializeField] protected Image songImage;

        // Start is called before the first frame update
        protected virtual void Start()
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

    }
}
