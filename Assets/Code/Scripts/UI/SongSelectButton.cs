using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public class SongSelectButton : SongCoverDisplay
    {
        public static event Action<SongData> OnSongSelectButtonClick;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

        }

        public void OnButtonClick()
        {
            OnSongSelectButtonClick?.Invoke(_songData);
        }

    }
}
