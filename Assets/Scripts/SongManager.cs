using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{

    [SerializeField] float songBpm;

    //the current position of the song (in seconds)
    [SerializeField] float songPosition;

    [SerializeField]
    //the current position of the song (in beats)
    public float songPosInBeats;

    //the duration of a beat
    [SerializeField] float secPerBeat;

    //how much time (in seconds) has passed since the song started
    float dspSongTime;

    // Start is called before the first frame update
    void Start()
    {

        //calculate how many seconds is one beat
        secPerBeat = 60f / songBpm;

        //record the time when the song starts
        dspSongTime = (float)AudioSettings.dspTime;

        //start the song
        GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        //calculate the position in seconds
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;

    }
}
