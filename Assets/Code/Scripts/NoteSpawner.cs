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
        [SerializeField] GameObject _normalNote;
        [SerializeField] GameObject _headNote;
        [SerializeField] GameObject _tailNote;

        [Header("Track 1 start transform")]
        [SerializeField] Transform[] track1Transform = new Transform[4];

        [Header("Track 2 start transform")]
        [SerializeField] Transform[] track2Transform = new Transform[4];

        [Header("Track 1 end transform")]
        [SerializeField] Transform[] track1EndTransform = new Transform[4];

        [Header("Track 2 end transform")]
        [SerializeField] Transform[] track2EndTransform = new Transform[4];


        // Start is called before the first frame update
        void Start()
        {
            //spawn notes on line 1
            foreach (NoteData noteData in _songData.notesLine1)
            {
                if (noteData.endBeat == 0)
                {
                    var instantateObject = Instantiate(_normalNote, track1Transform[noteData.track - 1]);

                    Note noteComponent = instantateObject.GetComponent<Note>();

                    noteComponent.NoteData = noteData;
                    noteComponent.StartTransform = track1Transform[noteData.track - 1];
                    noteComponent.EndTransform = track1EndTransform[noteData.track - 1];

                }
                else
                {
                    var headNote = Instantiate( _headNote, track1Transform[noteData.track - 1]);
                    var tailNote = Instantiate( _tailNote, track1Transform[noteData.track - 1]);

                    LongNote headNoteComponent = headNote.GetComponent<LongNote>();
                    Note tailNoteComponent = tailNote.GetComponent<Note>();

                    //assign head note component
                    headNoteComponent.NoteData = noteData;
                    headNoteComponent.StartTransform = track1Transform[noteData.track - 1];
                    headNoteComponent.EndTransform = track1EndTransform[noteData.track - 1];

                    //assign tail note component
                    NoteData tailData = new NoteData();
                    tailData.beat = noteData.endBeat;
                    tailData.track = noteData.track;
                    tailData.endBeat = 0;

                    tailNoteComponent.NoteData = tailData;
                    tailNoteComponent.StartTransform = track1Transform[noteData.track - 1];
                    tailNoteComponent.EndTransform = track1EndTransform[noteData.track - 1];

                    //set line
                    headNoteComponent.TailNoteTransform = tailNote.transform;

                }
            }
        }

    }
}
