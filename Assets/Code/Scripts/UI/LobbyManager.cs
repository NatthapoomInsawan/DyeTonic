using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            //delete all current room
            foreach (Transform roomTransform in roomListTransform.GetComponentInChildren<Transform>())
                Destroy(roomTransform);

            //update all room
            foreach (RoomInfo room in roomList)
            {
                var roomObject = Instantiate(roomObjectPrefab, roomListTransform);

                var roomButtonComponent = roomObject.GetComponent<RoomSelectButton>();

                roomButtonComponent.RoomName = room.Name;
                roomButtonComponent.UpdateSongCoverDisplay(Resources.Load<SongData>("SongData/" + room.CustomProperties["SongData"]));
            }
        }
    }
}
