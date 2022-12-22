using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{

    SongManager songManager;

    [SerializeField] float songBpm;

    //notes
    [SerializeField] List<float> notes = new List<float>();

    [SerializeField] int beatsShownInAdvance;

    [SerializeField] GameObject[] noteObjects;

    //track position
    [SerializeField] Transform track1;

    //end positiom
    [SerializeField] Transform endTransform;


    //the index of the next note to be spawned
    int nextIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        songManager = GetComponent<SongManager>();

        for (int i = 0; i < noteObjects.Length; i++)
        {
            notes.Add(noteObjects[i].GetComponent<Note>().beat);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (nextIndex < notes.Count && notes[nextIndex] < songManager.songPosInBeats + beatsShownInAdvance)
        {

            //initialize the fields of the music note
            Instantiate(noteObjects[nextIndex], noteObjects[nextIndex].GetComponent<Note>().spawnTransform);

            nextIndex++;
        }
    }
}
