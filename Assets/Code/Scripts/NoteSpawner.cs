using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class NoteSpawner : MonoBehaviour
    {
        [Header("Scriptable Objects referencing")]
        [SerializeField] SongManager _songManager;
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

        private void Awake()
        {
            //if songdata is null load current songdata from songManager
            if (_songData == null)
                _songData = _songManager.currentSongData;
        }

        // Start is called before the first frame update
        void Start()
        {
            SpawnNoteTwoLine();
        }

        public void SpawnNoteTwoLine()
        {
            //spawn notes on line 1
            SpawnNote(track1Transform, track1EndTransform, _songData.notesLine1);

            //spawn notes on line 2
            SpawnNote(track2Transform, track2EndTransform, _songData.notesLine2);
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

                    //Name the note
                    NoteNaming(instantateObject, noteData, noteDatas, trackTransforms);

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

                    //Name the note
                    NoteNaming(headNote, noteData, noteDatas, trackTransforms);

                }
            }
        }

        void NoteNaming(GameObject gameObject, NoteData noteData, List<NoteData> noteDatas, Transform[] transforms)
        {
            if (transforms == track1Transform)
                gameObject.name = "Line 1 index " + noteDatas.IndexOf(noteData);
            else
                gameObject.name = "Line 2 index " + noteDatas.IndexOf(noteData);

        }

        void ClearNotes(Transform[] transforms)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                Transform[] childGameObjects = transforms[i].GetComponentsInChildren<Transform>();

                for (int j = 0; j < childGameObjects.Length; j++)
                {
                    if (childGameObjects[j].GetComponent<Note>() != null)
                        Destroy(childGameObjects[j].gameObject);
                }
            }
        }

        public void ClearAllNotes()
        {
            ClearNotes(track1Transform);
            ClearNotes(track2Transform);
        }

    }
}
