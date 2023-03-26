using Photon.Pun;
using Photon.Realtime;
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
        [Range(1, 4)]
        [SerializeField] private int track = 1;

        [Header("Effect Prefab")]
        [SerializeField] private GameObject offbeatHit;
        [SerializeField] private GameObject perfectHit;
        [SerializeField] private GameObject goodHit;
        [SerializeField] private GameObject missHit;

        private float onPressBeat;

        private bool wasPressed;

        private int longNoteBeatCount;

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
        public static event Action<int, int, int, bool, bool> OnNetworkDataSend;
        public static event Action<float, int, bool> OnNoteRemove;
        public static event Action<int, int> OnLongNoteBeatDataSend;

        private void OnEnable()
        {
            Note.OnNoteSelfDestroy += NoteSelfDestroy;
            NetworkGameplayManager.OnRecieveNetworkDatas += RecieveNetworkDatas;
        }

        private void OnDisable()
        {
            Note.OnNoteSelfDestroy -= NoteSelfDestroy;
            NetworkGameplayManager.OnRecieveNetworkDatas -= RecieveNetworkDatas;
        }


        private void Update()
        {
            //Draw line when held input
            if (wasPressed)
                Debug.DrawLine(transform.position + new Vector3(0, 0, -2.5f), transform.position + new Vector3(0, 0, 2.5f), Color.red);

            if (hitLongNote != null && (_songManager.songPosInBeats >= hitLongNote.NoteData.endBeat))
            {
                SendLongNoteBeatCount();
                hitLongNote = null;
            }

            //Update long note press score
            if (hitLongNote != null && wasPressed == true)
            {
                if (_songManager.songPosInBeats - onPressBeat > 0.5f)
                {

                    //increse song combo
                    _songManager.songCombo++;

                    //long note beat count
                    longNoteBeatCount++;

                    AssignScore(50, hitLongNoteQality, player);

                    //invoke quality
                    InvokeNoteQualityEvent(hitLongNoteQality);

                    //update onPressBeat
                    onPressBeat = _songManager.songPosInBeats;

                    //invoke NetworkEvent
                    if (PhotonNetwork.InRoom)
                        SendNetworkData(50, hitLongNoteQality, player);

                    //spawn effect
                    SpawnEffect(hitLongNoteQality);

                    Debug.Log("long note press update");
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
                        longnoteComponent.HitByTrigger = true;

                        //assign hitlongNote reference to calculate when release note
                        hitLongNote = longnoteComponent;
                        hitLongNoteQality = CalculateBeatQuality(longnoteComponent.NoteData);
                        Debug.Log(hitLongNoteQality);
                        //assign onPressbeat
                        onPressBeat = _songManager.songPosInBeats;

                        //calculate score
                        CalculateScore(hitLongNoteQality);
                    }
                    else if (normalnoteComponent != null)
                    {
                        if (context.action.WasPressedThisFrame())
                        {
                            Debug.Log(CalculateBeatQuality(normalnoteComponent.NoteData));

                            //send network note delete
                            if (PhotonNetwork.InRoom)
                                SendNoteRemove(normalnoteComponent.NoteData);

                            //calculate score
                            CalculateScore(CalculateBeatQuality(normalnoteComponent.NoteData));
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

                    //send long note beat
                    if (PhotonNetwork.InRoom)
                        SendLongNoteBeatCount();

                    //calculate score
                    CalculateScore(hitLongNoteQality);
                    //Update to UI
                    OnNoteQualityUpdate?.Invoke(hitLongNoteQality);
                    OnNoteMiss?.Invoke();
                    Destroy(hitLongNote.gameObject);
                }
                else if (hitLongNote != null && _songManager.songPosInBeats > hitLongNote.NoteData.endBeat)
                {
                    //send long note beat
                    if (PhotonNetwork.InRoom)
                        SendLongNoteBeatCount();
                }
            }
        }

        private void CalculateScore(NoteQuality noteQuality)
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
            score *= _songManager.scoreMultiplier;

            //invoke NetworkEvent
            if (PhotonNetwork.InRoom)
                SendNetworkData(score, noteQuality, player);

            AssignScore(score, noteQuality, player);

        }

        private NoteQuality CalculateBeatQuality(NoteData noteData)
        {
            //Calculate error value
            float errorValue = (_songManager.songPosInBeats - noteData.beat) / noteData.beat * 100;
            NoteQuality noteQuality = NoteQuality.Miss;

            if (errorValue < 0 && errorValue < -2.5f)
                noteQuality = NoteQuality.Offbeat;
            else if (Mathf.Abs(errorValue) < 1.5f)
                noteQuality = NoteQuality.Perfect;
            else if (Mathf.Abs(errorValue) < 2.5f)
                noteQuality = NoteQuality.Good;

            //Update to UI
            InvokeNoteQualityEvent(noteQuality);

            //spawn effect
            SpawnEffect(noteQuality);

            return noteQuality;


        }

        private void InvokeNoteQualityEvent(NoteQuality noteQuality)
        {
            //Update to UI
            OnNoteQualityUpdate?.Invoke(noteQuality);

            if (noteQuality == NoteQuality.Miss)
                OnNoteMiss?.Invoke();

            if (noteQuality == NoteQuality.Good || noteQuality == NoteQuality.Perfect)
                OnNoteHit?.Invoke();
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

        private void AssignScore(int score, NoteQuality noteQuality, Player player)
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

        public void SetHitTriggerActive(bool setting)
        {
            isActive = setting;
        }

        public bool GetHitTriggerActive()
        {
            return isActive;
        }

        private void SendNetworkData(int score, NoteQuality noteQuality, Player player)
        {
            int qualityNumber = 0;
            bool isPlayer1;
            bool isSongCombo;

            //convert note quality to int
            switch (noteQuality)
            {
                case NoteQuality.Offbeat:
                    qualityNumber = 0;
                    break;
                case NoteQuality.Perfect:
                    qualityNumber = 1;
                    break;
                case NoteQuality.Good:
                    qualityNumber = 2;
                    break;
                case NoteQuality.Miss:
                    qualityNumber = 3;
                    break;
            }

            //convert player to boolean
            if (player == Player.Player1)
                isPlayer1 = true;
            else
                isPlayer1 = false;

            //check if song is combo
            if (_songManager.songCombo != 0)
                isSongCombo = true;
            else
                isSongCombo = false;

            Debug.Log("send network data");

            OnNetworkDataSend?.Invoke(score, qualityNumber, track, isSongCombo, isPlayer1);

        }

        private void RecieveNetworkDatas(int score, NoteQuality noteQuality, int trackData, bool isSongCombo, bool isPlayer1)
        {
            Player playerData;

            if (isPlayer1)
                playerData = Player.Player1;
            else
                playerData = Player.Player2;

            if (playerData != player || track != trackData)
                return;

            //assign song combo
            if (isSongCombo && score != 50)
                _songManager.songCombo++;
            else if (score != 50)
                _songManager.songCombo = 0;

            SpawnEffect(noteQuality);

            InvokeNoteQualityEvent(noteQuality);

            AssignScore(score, noteQuality, playerData);

            Debug.Log("recieved network data");
        }

        private void SendNoteRemove(NoteData noteData)
        {
            bool isPlayer1;

            if (player != Player.Player1)
                isPlayer1 = false;
            else
                isPlayer1 = true;

            OnNoteRemove?.Invoke(noteData.beat, noteData.track, isPlayer1);
        }

        private void SendLongNoteBeatCount()
        {
            int qualityNumber = 0;

            //convert note quality to int
            switch (hitLongNoteQality)
            {
                case NoteQuality.Offbeat:
                    qualityNumber = 0;
                    break;
                case NoteQuality.Perfect:
                    qualityNumber = 1;
                    break;
                case NoteQuality.Good:
                    qualityNumber = 2;
                    break;
                case NoteQuality.Miss:
                    qualityNumber = 3;
                    break;
            }

            OnLongNoteBeatDataSend?.Invoke(longNoteBeatCount, qualityNumber);

            longNoteBeatCount = 0;
        }

    }
}
