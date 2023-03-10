using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private Transform roomListTransform;
        [SerializeField] private GameObject roomObjectPrefab;

        // Start is called before the first frame update
        void Start()
        {
            playerNameText.text = PhotonNetwork.NickName;
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Connected to lobby!");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            //delete all current room
            foreach (Transform roomTransform in roomListTransform.GetComponentInChildren<Transform>())
                Destroy(roomTransform.gameObject);

            //update all room
            foreach (RoomInfo room in roomList)
            {

                var roomObject = Instantiate(roomObjectPrefab, roomListTransform);

                var roomButtonComponent = roomObject.GetComponent<RoomSelectButton>();

                roomButtonComponent.RoomName = room.Name;

                if (room.CustomProperties["songDataName"] == null)
                    Destroy(roomObject);
                else
                    roomButtonComponent.UpdateSongCoverDisplay(Resources.Load<SongData>("SongData/" + room.CustomProperties["songDataName"]));

            }
        }
    }
}
