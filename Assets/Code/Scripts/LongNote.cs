using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [RequireComponent(typeof(LineRenderer))]
    public class LongNote : Note
    {

        LineRenderer lineRenderer;

        public Transform TailNoteTransform { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            MoveToEndPoint();

            DrawLine();
        }

        protected override void MoveToEndPoint()
        {
            //move to hit point
            transform.position = Vector3.LerpUnclamped(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));

            //Hide when pass or equal 1.5 beat
            if ((_songManager.songPosInBeats - NoteData.beat) >= 1.5f && DestoryWhenPassHitLine && GetComponent<MeshRenderer>().enabled == true)
            {
                GetComponent<MeshRenderer>().enabled = false;
                InVokeSelfDestroy();
            }

            //Destroy when endBeat pass or equal end beat
            if ((_songManager.songPosInBeats - NoteData.endBeat) >= 1.5f && DestoryWhenPassHitLine)
                Destroy(gameObject);

        }


        //Draw line between two head and tail
        public void DrawLine()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, TailNoteTransform.position);
        }

    }
}
