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
            SpawnNote(track1Transform, track1EndTransform, _songData.notesLine1);

            //spawn notes on line 2
            //SpawnNote(track2Transform, track2EndTransform, _songData.notesLine2);
        }

        void SpawnNote(Transform[] trackTransforms, Transform[] trackEndTransforms, List<NoteData> noteDatas)
        {
            //spawn notes
            foreach (NoteData noteData in noteDatas)
            {
                if (noteData.endBeat == 0)
                {
                    var instantateObject = Instantiate(_normalNote, trackTransforms[noteData.track - 1]);

                    Note noteComponent = instantateObject.GetComponent<Note>();

                    noteComponent.NoteData = noteData;
                    noteComponent.StartTransform = trackTransforms[noteData.track - 1];
                    noteComponent.EndTransform = trackEndTransforms[noteData.track - 1];

                }
                else
                {
                    var headNote = Instantiate(_headNote, trackTransforms[noteData.track - 1]);
                    var tailNote = Instantiate(_tailNote, trackEndTransforms[noteData.track - 1]);

                    LongNote headNoteComponent = headNote.GetComponent<LongNote>();
                    Note tailNoteComponent = tailNote.GetComponent<Note>();

                    //assign head note component
                    headNoteComponent.NoteData = noteData;
                    headNoteComponent.StartTransform = trackTransforms[noteData.track - 1];
                    headNoteComponent.EndTransform = trackEndTransforms[noteData.track - 1];

                    //assign tail note component
                    NoteData tailData = new NoteData();
                    tailData.beat = noteData.endBeat;
                    tailData.track = noteData.track;
                    tailData.endBeat = 0;

                    tailNoteComponent.NoteData = tailData;
                    tailNoteComponent.StartTransform = trackTransforms[noteData.track - 1];
                    tailNoteComponent.EndTransform = trackEndTransforms[noteData.track - 1];

                    //set line
                    headNoteComponent.TailNoteTransform = tailNote.transform;

                }
            }
        }

    }
}
