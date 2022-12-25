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
            base.Update();

            DrawLine();
        }

        //Draw line between two head and tail
        public void DrawLine()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, TailNoteTransform.position);
        }

    }
}
