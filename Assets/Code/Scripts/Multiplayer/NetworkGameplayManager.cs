using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class NetworkGameplayManager : MonoBehaviour, IOnEventCallback, IPunObservable
    {
        [Header("SongManager")]
        [SerializeField] SongManager _songManager;

        [Header("Player HitLines")]
        [SerializeField] private List<HitTrigger> player1HitTriggers;
        [SerializeField] private List<HitTrigger> player2HitTriggers;

        [Header("Track Transform")]
        [SerializeField] private List<Transform> track1Transform;
        [SerializeField] private List<Transform> track2Transform;


        //event coded
        private const byte SCORE_UPDATE_EVENT = 0;
        private const byte GAME_END_EVENT = 1;
        private const byte NOTE_REMOVE_EVENT = 2;
        private const byte LONG_NOTE_BEAT_EVENT = 3;

        //event
        public static event Action<int, NoteQuality, int, bool, bool> OnRecieveNetworkDatas;
        public static event Action OnComboUpdate;

        private void Awake()
        {
            if (!PhotonNetwork.InRoom)
                gameObject.SetActive(false);
        }

        private void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                if ((int)PhotonNetwork.LocalPlayer.CustomProperties["character"] == 0)
                    DisableHitTriggers(player1HitTriggers);
                else
                    DisableHitTriggers(player2HitTriggers);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            HitTrigger.OnNetworkDataSend += InvokePhotonEvent;
            HitTrigger.OnNoteRemove += InvokeNoteRemoveEvent;
            HitTrigger.OnLongNoteBeatDataSend += LongNoteBeat;
            SongPlayer.OnGameEnd += GameEnd; 
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            HitTrigger.OnNetworkDataSend -= InvokePhotonEvent;
            HitTrigger.OnNoteRemove -= InvokeNoteRemoveEvent;
            HitTrigger.OnLongNoteBeatDataSend -= LongNoteBeat;
            SongPlayer.OnGameEnd -= GameEnd;
        }

        private void GameEnd()
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(GAME_END_EVENT, null, raiseEventOptions, SendOptions.SendReliable);
        }

        private void InvokePhotonEvent(int score, int qualityNumber, int track, bool isSongCombo,bool isPlayer1)
        {
            object[] data = new object[] { score, qualityNumber, track, isSongCombo, isPlayer1 };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SCORE_UPDATE_EVENT, data, raiseEventOptions, SendOptions.SendReliable);
        }
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == SCORE_UPDATE_EVENT)
            {
                object[] data = (object[])photonEvent.CustomData;
                int score = (int)data[0];
                int qualityNumber = (int)data[1];
                int track = (int)data[2];
                bool isSongCombo = (bool)data[3];
                bool isPlayer1 = (bool)data[4];

                NoteQuality noteQuality = NoteQuality.Miss;

                switch (qualityNumber)
                {
                    case 0:
                        noteQuality = NoteQuality.Offbeat;
                        break;
                    case 1:
                        noteQuality = NoteQuality.Perfect;
                        break;
                    case 2:
                        noteQuality = NoteQuality.Good;
                        break;
                    case 3:
                        noteQuality = NoteQuality.Miss;
                        break;
                }

                //Invoke event
                OnRecieveNetworkDatas?.Invoke(score, noteQuality, track, isSongCombo, isPlayer1);

            }
            
            if (eventCode == GAME_END_EVENT)
            {
                PhotonNetwork.LoadLevel("GameOverScene");
            }

            if (eventCode == NOTE_REMOVE_EVENT)
            {
                object[] data = (object[])photonEvent.CustomData;
                
                if ((bool)data[2])
                    NoteRemove((float)data[0], (int)data[1], track1Transform);
                else
                    NoteRemove((float)data[0], (int)data[1], track2Transform);
            }

            if (eventCode == LONG_NOTE_BEAT_EVENT)
            {
                object[] data = (object[])photonEvent.CustomData;

                int qualityNumber = (int)data[1];
                NoteQuality noteQuality = NoteQuality.Offbeat;

                switch (qualityNumber)
                {
                    case 0:
                        noteQuality = NoteQuality.Offbeat;
                        break;
                    case 1:
                        noteQuality = NoteQuality.Perfect;
                        break;
                    case 2:
                        noteQuality = NoteQuality.Good;
                        break;
                    case 3:
                        noteQuality = NoteQuality.Miss;
                        break;
                }

                if (noteQuality == NoteQuality.Miss)
                    _songManager.songCombo = 0;
                else
                    _songManager.songCombo += (int)data[0];

                OnComboUpdate?.Invoke();

                Debug.Log("song combo: " + _songManager.songCombo);

            }

        }

        private void DisableHitTriggers (List<HitTrigger> hitTriggers)
        {
            foreach (HitTrigger hitTrigger in hitTriggers)
            {
                hitTrigger.SetHitTriggerActive(false);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_songManager.HP);
            }
            else if (stream.IsReading)
            {
                _songManager.HP = (int)stream.ReceiveNext();
            }
        }

        private void InvokeNoteRemoveEvent(float beat, int track, bool isPlayer1)
        {
            object[] data = new object[] { beat, track, isPlayer1 };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(NOTE_REMOVE_EVENT, data, raiseEventOptions,SendOptions.SendUnreliable);
        }

        private void NoteRemove(float beat, int track, List<Transform> trackTransform)
        {
            foreach (Transform transform in trackTransform[track - 1].GetComponentInChildren<Transform>())
            {
                Note note;

                if (transform.GetComponent<NormalNote>() != null)
                    note = transform.GetComponent<NormalNote>();
                else if (transform.GetComponent<LongNote>() != null)
                    note = transform.GetComponent<LongNote>();
                else
                    note = null;

                if (note != null && note.NoteData.beat == beat)
                    Destroy(transform.gameObject);

            }
        }

        private void LongNoteBeat(int beat, int beatQuality)
        {
            object[] datas = new object[] { beat, beatQuality };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(LONG_NOTE_BEAT_EVENT, datas, raiseEventOptions, SendOptions.SendReliable);
        }

    }
}
