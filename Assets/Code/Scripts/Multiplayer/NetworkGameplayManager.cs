using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class NetworkGameplayManager : MonoBehaviour, IOnEventCallback
    {
        [Header("SongManager")]
        [SerializeField] SongManager _songManager;

        [Header("Player HitLines")]
        [SerializeField] private List<HitTrigger> player1HitTriggers;
        [SerializeField] private List<HitTrigger> player2HitTriggers;
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
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            throw new System.NotImplementedException();
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
