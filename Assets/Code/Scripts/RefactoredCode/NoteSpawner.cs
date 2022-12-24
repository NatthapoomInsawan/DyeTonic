using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [RequireComponent(typeof(SongPlayer))]
    public class NoteSpawner : MonoBehaviour
    {
        [Header("Scriptable Objects referencing")]
        [SerializeField] SongData _songData;

        [Header("Note Prefabs")]
        [SerializeField] GameObject _normalNotes;

        [Header("Track1 Position")]
        [SerializeField] Transform[] track1Transform = new Transform[4];

        [Header("Track2 Position")]
        [SerializeField] Transform[] track2Transform = new Transform[4];


        // Start is called before the first frame update
        void Start()
        {
            //spawn notes on line 1
            foreach (var noteData in _songData.notesLine1)
            {
                if (noteData.endBeat == 0)
                {
                    var instantateObject = _normalNotes;

                    Instantiate(instantateObject, track1Transform[noteData.track - 1]);

                }
            }
        }

    }
}
