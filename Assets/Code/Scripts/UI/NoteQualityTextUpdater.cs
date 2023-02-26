using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DyeTonic
{
    public class NoteQualityTextUpdater : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;

        private void Start()
        {
            textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            textMeshPro.enabled = false;
        }


        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnNoteQualityUpdate += UpdateUI;

        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnNoteQualityUpdate -= UpdateUI;

        }

        private void UpdateUI(NoteQuality noteQuality)
        {
            textMeshPro.text = noteQuality.ToString();
            textMeshPro.enabled = true;

            //tween animation
            TweenSequenceAnimation.PopSequence(transform, 1.2f, 0.1f);
            transform.DOShakePosition(0.2f, 2f, 10, 0);
        }

    }
}
