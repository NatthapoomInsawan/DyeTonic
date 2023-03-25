using Fungus;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class NoteSpawner : MonoBehaviour
    {
        [Header("Scriptable Objects referencing")]
        [SerializeField] private SongManager _songManager;
        [SerializeField] private SongData _songData;

        [Header("Spawn note option")]
        [SerializeField] private bool noteSelfDestroy = true;

        [Header("Note Prefabs")]
        [SerializeField] private GameObject _normalNote;
        [SerializeField] private GameObject _headNote;
        [SerializeField] private GameObject _tailNote;
        [SerializeField] private GameObject _normalNoteVariant;
        [SerializeField] private GameObject _headNoteVariant;

        [Header("Track 1 start transform")]
        [SerializeField] private Transform[] track1Transform = new Transform[4];

        [Header("Track 2 start transform")]
        [SerializeField] private Transform[] track2Transform = new Transform[4];

        [Header("Track 1 end transform")]
        [SerializeField] private Transform[] track1EndTransform = new Transform[4];

        [Header("Track 2 end transform")]
        [SerializeField] private Transform[] track2EndTransform = new Transform[4];

        private void Awake()
        {
            //if songdata is null load current songdata from songManager
            if (_songData == null)
                _songData = _songManager.GetCurrentSongData();
        }

        // Start is called before the first frame update
        private void Start()
        {
            SpawnNoteTwoLine();
        }

        public void SpawnNoteTwoLine()
        {
            //spawn notes on line 1
            SpawnNote(track1Transform, track1EndTransform, _songData.notesLine1, _normalNote, _headNote);

            //spawn notes on line 2
            SpawnNote(track2Transform, track2EndTransform, _songData.notesLine2, _normalNoteVariant, _headNoteVariant);
        }

        private void SpawnNote(Transform[] trackTransforms, Transform[] trackEndTransforms, List<NoteData> noteDatas, GameObject normalNotePrefab, GameObject headNotePrefab)
        {
            //spawn notes
            foreach (NoteData noteData in noteDatas)
            {
                if (noteData.endBeat == 0)
                {
                    var instantateObject = Instantiate(normalNotePrefab, trackTransforms[noteData.track - 1]);

                    Note noteComponent = instantateObject.GetComponent<Note>();

                    noteComponent.NoteData = noteData;
                    noteComponent.StartTransform = trackTransforms[noteData.track - 1];
                    noteComponent.EndTransform = trackEndTransforms[noteData.track - 1];

                    //set note self destroy
                    noteComponent.DestoryWhenPassHitLine = noteSelfDestroy;

                    //Name the note
                    NoteNaming(instantateObject, noteData, noteDatas, trackTransforms);

                    //disable different note
                    SetNetworkNoteComponent(trackTransforms, noteComponent);

                }
                else
                {
                    var headNote = Instantiate(headNotePrefab, trackTransforms[noteData.track - 1]);
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

                    //set note self destroy
                    headNoteComponent.DestoryWhenPassHitLine = noteSelfDestroy;

                    //Name the note
                    NoteNaming(headNote, noteData, noteDatas, trackTransforms);

                    //disable different note
                    SetNetworkNoteComponent(trackTransforms, headNoteComponent);

                }
            }
        }

        private void NoteNaming(GameObject gameObject, NoteData noteData, List<NoteData> noteDatas, Transform[] transforms)
        {
            if (transforms == track1Transform)
                gameObject.name = "Line 1 index " + noteDatas.IndexOf(noteData);
            else
                gameObject.name = "Line 2 index " + noteDatas.IndexOf(noteData);

        }

        private void ClearNotes(Transform[] transforms)
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

        private void SetNetworkNoteComponent (Transform[] trackTransforms, Note note)
        {
            if (!PhotonNetwork.InRoom)
                return;

            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["character"] == 0 && trackTransforms == track1Transform)
            {
                note.DestoryWhenPassHitLine = false;
            }
            else if ((int)PhotonNetwork.LocalPlayer.CustomProperties["character"] == 1 && trackTransforms == track2Transform)
            {
                note.DestoryWhenPassHitLine = false;
            }

        }

        public void ClearAllNotes()
        {
            ClearNotes(track1Transform);
            ClearNotes(track2Transform);
        }

    }
}
