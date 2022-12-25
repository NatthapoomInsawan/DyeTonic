using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DyeTonic
{
    public abstract class Note : MonoBehaviour
    {
        [SerializeField] SongManager _songManager;

        //reach where it need to be hit
        bool reachHitLine;

        public Transform StartTransform { get; set; }
        public Transform EndTransform { get; set; }
        public NoteData NoteData { get; set; }

        // Update is called once per frame
        protected virtual void Update()
        {
            MoveToEndPoint();

        }

        protected virtual void MoveToEndPoint()
        {
            //move to end point
            if (transform != EndTransform && !reachHitLine)
                transform.position = Vector3.Lerp(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));

            //reached end point
            if (transform.position == EndTransform.position)
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
}
