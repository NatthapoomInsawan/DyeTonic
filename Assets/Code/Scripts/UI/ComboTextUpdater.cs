using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DyeTonic
{
    public class ComboTextUpdater : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        [SerializeField] private SongManager _songManager;

        private void Start()
        {
            textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            textMeshPro.enabled = false;
        }


        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnScoreUpdate += UpdateUI;
            Note.OnNoteSelfDestroy += UpdateUI;
        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnScoreUpdate -= UpdateUI;
            Note.OnNoteSelfDestroy -= UpdateUI;
        }

        void UpdateUI()
        {
            textMeshPro.text = _songManager.songCombo.ToString();
            textMeshPro.enabled = true;

            //tween animation
            TweenSequenceAnimation.PopSequence(transform, 1.5f, 0.1f);
        }

    }
}
