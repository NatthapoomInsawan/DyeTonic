using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

namespace DyeTonic
{
    public enum NoteQuality
    {
        Offbeat,
        Good,
        Perfect,
        Miss,
    }

    public class HitTrigger : MonoBehaviour
    {
        [Header("HitTrigger settings")]
        [SerializeField] private SongManager _songManager;
        [SerializeField] private Player player = Player.Player2;
        [SerializeField] private bool isActive = true;

        [Header("Effect Prefab")]
        [SerializeField] private GameObject offbeatHit;
        [SerializeField] private GameObject perfectHit;
        [SerializeField] private GameObject goodHit;
        [SerializeField] private GameObject missHit;

        private float onPressBeat;

        private bool wasPressed;

        LongNote hitLongNote;
        NoteQuality hitLongNoteQality;

        //enum
        private enum Player
        {
            Player1,
            Player2,
        }

        //Declare Events
        public static event Action OnScoreUpdate;
        public static event Action<NoteQuality> OnNoteQualityUpdate;
        public static event Action OnNoteMiss;
        public static event Action OnNoteHit;

        private void OnEnable()
        {
            Note.OnNoteSelfDestroy += NoteSelfDestroy;
        }

        private void OnDisable()
        {
            Note.OnNoteSelfDestroy -= NoteSelfDestroy;
        }


        private void Update()
        {
            //Draw line when held input
            if (wasPressed)
                Debug.DrawLine(transform.position + new Vector3(0, 0, -2.5f), transform.position + new Vector3(0, 0, 2.5f), Color.red);

            //Update long note press score
            if (hitLongNote != null && wasPressed == true)
            {
                if (_songManager.songPosInBeats - onPressBeat > 0.5f) 
                {
                    AssignScore(50, hitLongNoteQality);

                    //increse song combo
                    _songManager.songCombo++;

                    //invoke quality
                    OnNoteQualityUpdate?.Invoke(hitLongNoteQality);

                    //update onPressBeat
                    onPressBeat = _songManager.songPosInBeats;

                    //spawn effect
                    SpawnEffect(hitLongNoteQality);

                }
            }

        }

        public void ProcessTriggerInput(InputAction.CallbackContext context) 
        { 
            if (isActive)
            {
                if (context.performed)
                    OnKeypressed(context);
                if (context.canceled)
                    OnKeyRelease(context);
            }
        }

        private void OnKeypressed(InputAction.CallbackContext context)
        {

            Ray ray = new Ray(transform.position + new Vector3(0, 0, -2.5f), transform.forward);

            RaycastHit hit;

            if (context.action.WasPressedThisFrame())
            {
                wasPressed = true;
                if (Physics.Raycast(ray, out hit, 5))
                {
                    LongNote longnoteComponent = hit.transform.GetComponent<LongNote>();
                    NormalNote normalnoteComponent = hit.transform.GetComponent<NormalNote>();

                    if (longnoteComponent != null)
                    {
                        //assign hitlongNote reference to calculate when release note
                        hitLongNote = longnoteComponent;
                        hitLongNoteQality = CalculateBeatQuality(longnoteComponent.NoteData);
                        Debug.Log(hitLongNoteQality);
                        //assign onPressbeat
                        onPressBeat = _songManager.songPosInBeats;

                        //calculate score
                        CalculateScore( hitLongNoteQality);
                    }
                    else if (normalnoteComponent != null)
                    {
                        if (context.action.WasPressedThisFrame())
                        {
                            Debug.Log(CalculateBeatQuality(normalnoteComponent.NoteData));
                            CalculateScore(hitLongNoteQality);
                            //calculate score
                            Destroy(hit.transform.gameObject);
                        }

                    }

                }
            }
        }

        private void OnKeyRelease(InputAction.CallbackContext context)
        {
            if (context.action.WasReleasedThisFrame())
            {
                wasPressed = false;

                //check if longnote release before it should
                if (hitLongNote != null && _songManager.songPosInBeats < hitLongNote.NoteData.endBeat)
                {
                    hitLongNoteQality = NoteQuality.Miss;
                    Debug.Log(hitLongNoteQality);
                    //calculate score
                    CalculateScore(hitLongNoteQality);
                    //Update to UI
                    OnNoteQualityUpdate?.Invoke(hitLongNoteQality);
                    OnNoteMiss?.Invoke();
                    Destroy(hitLongNote.gameObject);
                }
            }
        }

        private void CalculateScore (NoteQuality noteQuality)
        {
            int score = 0;

            //assign score
            switch (noteQuality)
            {
                case NoteQuality.Good:
                    score += 100;
                    break;
                case NoteQuality.Perfect:
                    score += 150;
                    break;
                default:
                    score = 0;
                    break;
            }

            //check if combo is break
            if (noteQuality == NoteQuality.Miss)
                _songManager.songCombo = 0;
            else
                _songManager.songCombo++;

            //check score multiplier by combo
            if (_songManager.songCombo > 200)
                _songManager.scoreMultiplier = 5;
            else if (_songManager.songCombo > 150)
                _songManager.scoreMultiplier = 4;
            else if (_songManager.songCombo > 100)
                _songManager.scoreMultiplier = 3;
            else if (_songManager.songCombo > 50)
                _songManager.scoreMultiplier = 2;
            else
                _songManager.scoreMultiplier = 1;

            //Multiply score
            score = score * _songManager.scoreMultiplier;

            AssignScore(score, noteQuality);

        }

        private NoteQuality CalculateBeatQuality(NoteData noteData)
        {
            //Calculate error value
            float errorValue = (_songManager.songPosInBeats-noteData.beat)/noteData.beat * 100;
            NoteQuality noteQuality = NoteQuality.Miss;

            if (errorValue < 0 && errorValue < -2.5f)
                noteQuality = NoteQuality.Offbeat;
            else if ( Mathf.Abs(errorValue) < 1.5f)
                noteQuality = NoteQuality.Perfect;
            else if (Mathf.Abs(errorValue) < 2.5f)
                noteQuality = NoteQuality.Good;

            //Update to UI
            OnNoteQualityUpdate?.Invoke(noteQuality);
            
            if (noteQuality == NoteQuality.Miss)
                OnNoteMiss?.Invoke();

            if (noteQuality == NoteQuality.Good || noteQuality == NoteQuality.Perfect)
                OnNoteHit?.Invoke();

            //spawn effect
            SpawnEffect(noteQuality);

            return noteQuality;


        }

        private void UpdateHitNotes(int[] hitNotesArray, NoteQuality noteQuality)
        {
            //update hit note
            switch (noteQuality)
            {
                case NoteQuality.Offbeat:
                    hitNotesArray[0] += 1;
                    break;
                case NoteQuality.Perfect:
                    hitNotesArray[1] += 1;
                    break;
                case NoteQuality.Good:
                    hitNotesArray[2] += 1;
                    break;
                case NoteQuality.Miss:
                    hitNotesArray[3] += 1;
                    break;
            }
        }

        private void AssignScore (int score, NoteQuality noteQuality)
        {
            //assign score to player
            if (player == Player.Player2)
            {
                _songManager.player2Score += score;

                UpdateHitNotes(_songManager.play2HitNotes, noteQuality);
            }
            else
            {
                _songManager.player1Score += score;

                UpdateHitNotes(_songManager.play1HitNotes, noteQuality);
            }

            //Invoke OnScoreUpdate event
            OnScoreUpdate?.Invoke();

        }

        private void SpawnEffect(NoteQuality noteQuality)
        {
            //update hit note
            switch (noteQuality)
            {
                case NoteQuality.Offbeat:
                    Instantiate(offbeatHit, transform);
                    break;
                case NoteQuality.Perfect:
                    Instantiate(perfectHit, transform);
                    break;
                case NoteQuality.Good:
                    Instantiate(goodHit, transform);
                    break;
                case NoteQuality.Miss:
                    Instantiate(missHit, transform);
                    break;
            }
        }

        private void NoteSelfDestroy(Transform targetTransform, NoteData noteData)
        {
            if (targetTransform != transform)
                return;

            CalculateBeatQuality(noteData);
            CalculateScore(NoteQuality.Miss);

        }

        public void SetHitTriggerActive (bool setting)
        {
            isActive = setting;
        }

    }
}
