using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class SongListLoader : MonoBehaviour
    {
        [SerializeField] private SongData[] songDatas;
        [SerializeField] private GameObject songButtonPrefab;
        [SerializeField] private Transform contentContainer;

        private void Awake()
        {
            songDatas = Resources.LoadAll<SongData>("SongData");
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < songDatas.Length; i++)
            {
                var instantiateObject = Instantiate(songButtonPrefab, contentContainer);

                //assign song data
                instantiateObject.GetComponent<SongSelectButton>().UpdateSongCoverDisplay(songDatas[i]);

                //assign cover to SongPanel
                //if (i == 0)
                //    instantiateObject.GetComponent<SongSelectButton>().OnButtonClick();

                //rename instantiateObject
                if (songDatas[i].songName != null)
                    instantiateObject.name = songDatas[i].songName;
                else
                    instantiateObject.name = songDatas[i].name;
            }
        }
    }
}
