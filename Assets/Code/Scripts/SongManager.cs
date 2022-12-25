using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Song Manager")]
    public class SongManager : ScriptableObject
    {

        [SerializeField]
        //the current position of the song (in beats)
        public float songPosInBeats;

    }
}
