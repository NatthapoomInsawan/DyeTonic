using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Song Data")]
    public class SongData : ScriptableObject
    {
        [Header("Song Information")]
        public string songName;
        public string songArtist;

        [Tooltip("Audio of song to play in map")]
        public AudioClip song;

        [Tooltip("Beat per minute of this song")]
        public float songBpm;

        [Tooltip("Image of the song")]
        public Sprite songSprite;

        [Header("Note Information")]
        [Tooltip("List of note to spawn in line 1")]
        public List<NoteData> notesLine1;

        [Tooltip("List of note to spawn in line 2")]
        public List<NoteData> notesLine2;

    }
}
