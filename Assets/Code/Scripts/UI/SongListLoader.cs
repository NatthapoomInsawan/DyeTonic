using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class SongListLoader : MonoBehaviour
    {
        [SerializeField] private SongData[] songDatas;

        private void Awake()
        {
            songDatas = Resources.LoadAll<SongData>("Resources/");
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }
    }
}
