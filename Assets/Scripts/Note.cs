using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform spawnTransform;
    [SerializeField] Transform endTransform;
    [SerializeField] int beatsShownInAdvance;
    public float beat;
    [SerializeField] float songPosInBeats;
    [SerializeField] GameObject songManager;

    bool reachHitLine;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        songPosInBeats = songManager.GetComponent<SongManager>().songPosInBeats;

        if (transform != endTransform && !reachHitLine)
        {
            transform.position = Vector3.Lerp(spawnTransform.position, endTransform.position, (songPosInBeats / beat));

            //Debug.Log((songPosInBeats / beat).ToString());
        }

        if (transform.position == endTransform.position)
        {
            reachHitLine = true;
            //Destroy(gameObject);
            //Debug.Log("PERFECT");
        }

        //if (reachHitLine)
        //    transform.position = Vector3.Lerp(endTransform.position, endTransform.position + new Vector3(0, 0, -2.5f), (songPosInBeats / (beat+10)));

        //Debug.Log((songPosInBeats / (beat+10)).ToString());
    }

}
