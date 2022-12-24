using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public abstract class Note : MonoBehaviour
    {
        [SerializeField] SongManager _songManager;

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
            transform.position = Vector3.Lerp(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));
        }

    }
}
