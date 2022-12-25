using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [RequireComponent(typeof(LineRenderer))]
    public class LongNote : Note
    {
        public LineRenderer LineRenderer { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
