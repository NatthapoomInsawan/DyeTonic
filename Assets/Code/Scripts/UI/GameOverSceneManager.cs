using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DyeTonic
{
    public class GameOverSceneManager : MonoBehaviourPunCallbacks
    {

        [Header("Scriptable Object")]
        [SerializeField] private SongManager _songManager;

        [Header("Panel")]
        [SerializeField] private Transform gameWinPanel;
        [SerializeField] private Transform gameLosePanel;
        [SerializeField] private Transform playAgainButton;

        [Header("Score")]
        [SerializeField] private TextMeshProUGUI resultScoreText;
        [Header("Player 1 Combo")]
        [SerializeField] private TextMeshProUGUI player1PerfectText;
        [SerializeField] private TextMeshProUGUI player1GoodText;
        [SerializeField] private TextMeshProUGUI player1OffBeatText;
        [SerializeField] private TextMeshProUGUI player1MissText;
        [Header("Player 2 Combo")]
        [SerializeField] private TextMeshProUGUI player2PerfectText;
        [SerializeField] private TextMeshProUGUI player2GoodText;
        [SerializeField] private TextMeshProUGUI player2OffBeatText;
        [SerializeField] private TextMeshProUGUI player2MissText;

        // Start is called before the first frame update
        void Start()
        {
            if (_songManager.HP <= 0)
                gameLosePanel.gameObject.SetActive(true);
            else
                gameWinPanel.gameObject.SetActive(true);

            if (PhotonNetwork.InRoom)
                playAgainButton.gameObject.SetActive(false);

            ShowScore();
        }

        public void OnPlayAgainButton()
        {
            SceneManager.LoadScene("GameplayScene");
        }

        public void OnGiveUpButton()
        {
            if (PhotonNetwork.InRoom)
                PhotonNetwork.LeaveRoom();
            else if (_songManager.IsStoryMode() != true)
                SceneLoader.LoadPreviousScene();
        }

        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("LobbyScene");
        }

        private void ShowScore()
        {
            resultScoreText.text = (_songManager.player1Score + _songManager.player2Score).ToString();

            player1PerfectText.text = _songManager.play1HitNotes[0].ToString();
            player1GoodText.text = _songManager.play1HitNotes[1].ToString();
            player1OffBeatText.text = _songManager.play1HitNotes[2].ToString();
            player1MissText.text = _songManager.play1HitNotes[3].ToString();

            player2PerfectText.text = _songManager.play2HitNotes[0].ToString();
            player2GoodText.text = _songManager.play2HitNotes[1].ToString();
            player2OffBeatText.text = _songManager.play2HitNotes[2].ToString();
            player2MissText.text = _songManager.play2HitNotes[3].ToString();
        }

    }
}
