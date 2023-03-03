using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace DyeTonic
{
    public class NetworkGameplayManager : MonoBehaviour, IOnEventCallback
    {
        [Header("SongManager")]
        [SerializeField] SongManager _songManager;

        [Header("Player HitLines")]
        [SerializeField] private List<HitTrigger> player1HitTriggers;
        [SerializeField] private List<HitTrigger> player2HitTriggers;

        //event coded
        private const byte SCORE_UPDATE_EVENT = 0;

        //event
        public static event Action<int, NoteQuality, int, bool, bool> OnRecieveNetworkDatas;

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
            Note.OnNoteSelfDestroy += NoteSelfDestroy;
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            HitTrigger.OnNetworkDataSend -= InvokePhotonEvent;
            Note.OnNoteSelfDestroy -= NoteSelfDestroy;
        }

        private void InvokePhotonEvent(int score, int qualityNumber, int track, bool isSongCombo,bool isPlayer1)
        {
            object[] datas = new object[] { score, qualityNumber, track, isSongCombo, isPlayer1 };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(NetworkGameplayManager.SCORE_UPDATE_EVENT, datas, raiseEventOptions, SendOptions.SendReliable);
        }

        private void NoteSelfDestroy(Transform transform, NoteData noteData)
        {
            bool isPlayer1;

            if (player2HitTriggers[0].GetHitTriggerActive())
                isPlayer1 = false;
            else
                isPlayer1 = true;

            InvokePhotonEvent(0, 3, noteData.track, false, isPlayer1);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == SCORE_UPDATE_EVENT)
            {
                object[] datas = (object[])photonEvent.CustomData;
                int score = (int)datas[0];
                int qualityNumber = (int)datas[1];
                int track = (int)datas[2];
                bool isSongCombo = (bool)datas[3];
                bool isPlayer1 = (bool)datas[4];

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

        }

        private void DisableHitTriggers (List<HitTrigger> hitTriggers)
        {
            foreach (HitTrigger hitTrigger in hitTriggers)
            {
                hitTrigger.SetHitTriggerActive(false);
            }
        }
    }
}
