using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DyeTonic
{
    public class ScoreTextUpdater : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        [SerializeField] private SongManager _songManager;

        private void Start()
        {
            textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        }


        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnScoreUpdate += UpdateUI;
        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnScoreUpdate -= UpdateUI;
        }

        void UpdateUI()
        {
            textMeshPro.text = (_songManager.player1Score + _songManager.player2Score).ToString();
        }
    }
}
