using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DyeTonic
{
    public abstract class Note : MonoBehaviour
    {
        [SerializeField] protected SongManager _songManager;

        [field: SerializeField] public bool DestoryWhenPassHitLine { get; set; } = true;
        public Transform StartTransform { get; set; }
        public Transform EndTransform { get; set; }
        public NoteData NoteData { get; set; }

        //OnNoteSelfDestroy
        public static event Action OnNoteSelfDestroy;

        // Update is called once per frame
        protected virtual void Update()
        {
            MoveToEndPoint();

        }

        protected virtual void MoveToEndPoint()
        {
            //move to hit point
            transform.position = Vector3.LerpUnclamped(StartTransform.position, EndTransform.position, (_songManager.songPosInBeats / NoteData.beat));

            //Destroy when pass or equal 1.5 beat
            if ((_songManager.songPosInBeats - NoteData.beat) >= 1.5f && DestoryWhenPassHitLine)
            {
                Destroy(gameObject);
                InVokeSelfDestroy();
            }

        }

        protected void InVokeSelfDestroy()
        {
            OnNoteSelfDestroy?.Invoke();
        }

    }
}
