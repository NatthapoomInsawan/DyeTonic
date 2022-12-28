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
            //move to hit    point
            transform.position = Vector3.LerpUnclamped(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));

            //Destroy when pass or equal 1.5 beat
            if ((_songManager.songPosInBeats - NoteData.beat) >= 1.5f)
                Destroy(gameObject);

        }

    }
}
