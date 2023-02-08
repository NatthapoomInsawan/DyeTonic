using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace DyeTonic
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {

        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button connectButton;

        public void OnConnectButtonClick()
        {
            connectButton.interactable = false;

            nameInputField.text = "Connecting..";
            
            //set player name
            PhotonNetwork.NickName = nameInputField.text;

            //connect to server
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log(PhotonNetwork.NickName + "connected to master server!");

            Debug.Log("Connecting to lobby..");

            //connect to lobby
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("connected to lobby!");
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log(returnCode + message);
            base.OnJoinRoomFailed(returnCode, message);
        }

    }
}
