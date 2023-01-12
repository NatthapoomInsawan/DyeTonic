using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] SongManager _songManager;

        bool wasPressed;
        LongNote hitLongNote;
        NoteQuality hitLongNoteQality;

        //Declare Events
        public static event Action OnScoreUpdate;
        public static event Action<NoteQuality> OnNoteQualityUpdate;

        private void Update()
        {
            //Draw line when held input
            if (wasPressed)
                Debug.DrawLine(transform.position + new Vector3(0, 0, -2.5f), transform.position + new Vector3(0, 0, 2.5f), Color.red);

        }

        public void Track1(InputAction.CallbackContext context) 
        { 
            if (context.performed)
                OnKeypressed(context);
            if (context.canceled)
                OnKeyRelease(context);
        }

        public void Track2 (InputAction.CallbackContext context)
        {
            if (context.performed)
                OnKeypressed(context);
            if (context.canceled)
                OnKeyRelease(context);
        }

        public void Track3 (InputAction.CallbackContext context)
        {
            if (context.performed)
                OnKeypressed(context);
            if (context.canceled)
                OnKeyRelease(context);
        }

        public void Track4 (InputAction.CallbackContext context)
        {
            if (context.performed)
                OnKeypressed(context);
            if (context.canceled)
                OnKeyRelease(context);
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
                        //calculate score
                        CalculateScore(context, hitLongNoteQality);
                    }
                    else if (normalnoteComponent != null)
                    {
                        if (context.action.WasPressedThisFrame())
                        {
                            Debug.Log(CalculateBeatQuality(normalnoteComponent.NoteData));
                            CalculateScore(context, hitLongNoteQality);
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
                    CalculateScore(context, hitLongNoteQality);
                    //Update to UI
                    OnNoteQualityUpdate?.Invoke(hitLongNoteQality);
                    Destroy(hitLongNote.gameObject);
                }
            }
        }

        private void CalculateScore (InputAction.CallbackContext context, NoteQuality noteQuality)
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

            //assign score to player
            if (context.action.actionMap.name == "Player2")
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

    }
}
