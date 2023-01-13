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
            Note.OnNoteSelfDestroy += NoteSelfDestroy;
        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnNoteQualityUpdate -= UpdateUI;
            Note.OnNoteSelfDestroy -= NoteSelfDestroy;
        }

        void UpdateUI(NoteQuality noteQuality)
        {
            textMeshPro.text = noteQuality.ToString();
            textMeshPro.enabled = true;
        }
        void NoteSelfDestroy()
        {
            UpdateUI(NoteQuality.Miss);
        }

    }
}
