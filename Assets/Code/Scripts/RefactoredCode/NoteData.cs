using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DyeTonic
{

    [System.Serializable]

    public class NoteData
    {
        [Tooltip("Position of this note in beat")]
        public float beat;

        [Tooltip("End position of long note (If it is single note set the value to 0)")]
        public float endBeat;
    }

}
