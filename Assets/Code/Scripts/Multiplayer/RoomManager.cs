using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DyeTonic
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Room Information")]
        [SerializeField] private SongData _roomSongData;
        [SerializeField] private TextMeshProUGUI songNameText;

        [Header("Player 1 Text")]
        [SerializeField] private TextMeshProUGUI player1NameText;
        [SerializeField] private TextMeshProUGUI player1ReadyText;

        [Header("Player 2 Text")]
        [SerializeField] private TextMeshProUGUI player2NameText;
        [SerializeField] private TextMeshProUGUI player2ReadyText;

        [Header("Button")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        [Header("Splash Art")]
        [SerializeField] private Sprite noneSplashArt;
        [SerializeField] private Sprite ciarSplashArt;
        [SerializeField] private Sprite lukeSplashArt;
        [SerializeField] private Image player1Splash;
        [SerializeField] private Image player2Splash;


        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            _roomSongData = Resources.Load<SongData>("SongData/" + PhotonNetwork.CurrentRoom.CustomProperties["songDataName"]);
            songNameText.text = _roomSongData.songName;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (targetPlayer.IsMasterClient)
            {
                player1NameText.text = targetPlayer.NickName;

                if (changedProps.ContainsKey("ready"))
                    SetReadyText(player1ReadyText, (bool)targetPlayer.CustomProperties["ready"]);

                if (changedProps.ContainsKey("character"))
                    SetSplashArt(player1Splash, (int)targetPlayer.CustomProperties["character"]);

            }
            else
            {
                player2NameText.text = targetPlayer.NickName;

                if (changedProps.ContainsKey("ready"))
                    SetReadyText(player2ReadyText, (bool)targetPlayer.CustomProperties["ready"]);

                if (changedProps.ContainsKey("character"))
                    SetSplashArt(player2Splash, (int)targetPlayer.CustomProperties["character"]);

            }

            Debug.Log(targetPlayer.NickName + " ready " + targetPlayer.CustomProperties["ready"]);
            Debug.Log(targetPlayer.NickName + " character " + targetPlayer.CustomProperties["character"]);

            //if player is equal 2
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Debug.Log("in condition");

                int readyPlayer = 0;

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if ((bool)player.CustomProperties["ready"] == true)
                        readyPlayer++;
                }

                //Start the game if player ready equal to 2
                if (readyPlayer == 2)
                {
                    buttonText.text = "Starting..";
                    button.interactable = false;
                    PhotonNetwork.LoadLevel("testGamplayUI");
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (newPlayer.IsMasterClient != true)
            {
                var playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
                playerCustomProperties.Add("ready", false);

                //set joined player property
                if ((int) PhotonNetwork.MasterClient.CustomProperties["character"] == 0)
                    playerCustomProperties.Add("character", 1);
                else
                    playerCustomProperties.Add("character", 0);

                newPlayer.SetCustomProperties(playerCustomProperties);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (otherPlayer.IsMasterClient == true)
            {
                player1NameText.text = "NONE";
                SetReadyText(player1ReadyText, false);
                player1Splash.sprite = noneSplashArt;
            }
            else
            {
                player2NameText.text = "NONE";
                SetReadyText(player2ReadyText, false);
                player2Splash.sprite = noneSplashArt;
            }
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            //leave the room when become master
            OnLeaveButton();
        }

        public override void OnJoinedRoom()
        {
            //show room property
            _roomSongData = Resources.Load<SongData>("SongData/" + PhotonNetwork.CurrentRoom.CustomProperties["songDataName"]);
            songNameText.text = _roomSongData.songName;

            //show master client property
            player1NameText.text = PhotonNetwork.MasterClient.NickName;
            SetReadyText(player1ReadyText, (bool)PhotonNetwork.MasterClient.CustomProperties["ready"]);
            SetSplashArt(player1Splash, (int)PhotonNetwork.MasterClient.CustomProperties["character"]);
        }

        private void SetReadyText (TextMeshProUGUI readyText, bool ready)
        {
            if (ready == true)
            {
                readyText.text = "READY";
                readyText.color = new Color32(51, 253, 100, 255);
            }
            else
            {
                readyText.text = "NOT READY";
                readyText.color = new Color32(253, 51, 51, 255);
            }
        }

        private void SetSplashArt (Image splashArtImage, int character)
        {
            switch(character)
            {
                case 0:
                    splashArtImage.sprite = ciarSplashArt;
                    break;
                case 1:
                    splashArtImage.sprite = lukeSplashArt;
                    break;
            }
        }

        public void OnReadyButton()
        {
            var playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["ready"] == false)
            {
                playerCustomProperties.Add("ready", true);
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
                buttonText.text = "NOT READY";
            }
            else
            {
                playerCustomProperties.Add("ready", false);
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
                buttonText.text = "READY";
            }
        }

        public void OnLeaveButton()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            SceneManager.LoadScene("LobbyScene");
        }

    }

}
