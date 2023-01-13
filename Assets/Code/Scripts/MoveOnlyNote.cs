using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class MoveOnlyNote : Note
    {
        protected override void MoveToEndPoint()
        {
            //move to hit    point
            transform.position = Vector3.LerpUnclamped(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));
        }
    }
}
