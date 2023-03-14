using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class RoomCreateManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int character;
        [SerializeField] private string songDataToLoad;

        public override void OnEnable()
        {
            SongSelectButton.OnSongSelectButtonClick += UpdateSongDataToLoad;

            base.OnEnable();
        }

        public override void OnDisable()
        {
            SongSelectButton.OnSongSelectButtonClick -= UpdateSongDataToLoad;

            base.OnDisable();
        }

        public void UpdateSongDataToLoad(SongData songData)
        {
            songDataToLoad = songData.name;
        }

        public void OnCharacterButtonClick(int index)
        {
            character = index;
        }

        public void OnRoomCreateButton()
        {
            //room options
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            string[] lobbyPropertyNames = new string[2];
            lobbyPropertyNames[0] = "songDataName";
            lobbyPropertyNames[1] = "gameStart";
            roomOptions.CustomRoomPropertiesForLobby = lobbyPropertyNames;

            //create room by creator name
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName, roomOptions);

        }

        public override void OnCreatedRoom()
        {
            //set room custom properties
            var roomCustomProperties = new ExitGames.Client.Photon.Hashtable();
            roomCustomProperties.Add("songDataName", songDataToLoad);
            roomCustomProperties.Add("gameStart", false);

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomCustomProperties);

            //add player custom properties
            var playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties.Add("character", character);
            playerCustomProperties.Add("ready", false);
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

            //load room scene
            SceneManager.LoadScene("RoomScene");

        }

    }
}
