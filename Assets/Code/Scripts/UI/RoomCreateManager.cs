using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class RoomCreateManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int character;
        private string songDataToLoad;

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

            //set room custom properties
            Hashtable roomCustomProperties = new Hashtable();
            roomCustomProperties.Add("songDataName", songDataToLoad);

            //create room by creator name
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName, roomOptions);

        }

        public override void OnCreatedRoom()
        {
            //add player custom properties
            var playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties.Add("character", character);
            playerCustomProperties.Add("ready", false);

            
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

        }


    }
}
