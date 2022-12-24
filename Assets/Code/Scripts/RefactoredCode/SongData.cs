using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Song Data")]
    public class SongData : ScriptableObject
    {
        [Tooltip("Audio of song to play in map")]
        public AudioClip song;

        [Tooltip("List of note to spawn")]
        public List<NoteData> notes;
    }
}
