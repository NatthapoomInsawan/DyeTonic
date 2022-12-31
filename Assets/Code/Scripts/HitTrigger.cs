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
        NoteQuality noteQuality;

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
                        //assign hiitlongNote reference to calculate when release note
                        hitLongNote = longnoteComponent;
                        noteQuality = CalculateBeatQuality(longnoteComponent.NoteData);
                        Debug.Log(noteQuality);
                    }
                    else if (normalnoteComponent != null)
                    {
                        if (context.action.WasPressedThisFrame())
                        {
                            Debug.Log(CalculateBeatQuality(normalnoteComponent.NoteData));
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
                if (hitLongNote != null && _songManager.songPosInBeats < hitLongNote.NoteData.endBeat)
                {
                    noteQuality = NoteQuality.Miss;
                    Debug.Log(noteQuality);
                    Destroy(hitLongNote.gameObject);
                }
            }
        }

        private void CalculateScore (InputAction.CallbackContext context)
        {

            if (context.action.actionMap.name == "Player2")
            {

            }
            else
            {

            }

        }

        private NoteQuality CalculateBeatQuality(NoteData noteData)
        {
            //Calculate error value
            float errorValue = (_songManager.songPosInBeats-noteData.beat)/noteData.beat * 100;
            NoteQuality noteQuality = NoteQuality.Miss;

            if (errorValue < 0)
                noteQuality = NoteQuality.Offbeat;
            else if ( Mathf.Abs(errorValue) < 1.5f)
                noteQuality = NoteQuality.Perfect;
            else if (Mathf.Abs(errorValue) < 2.5f)
                noteQuality = NoteQuality.Good;

            return noteQuality;

        }

    }
}
