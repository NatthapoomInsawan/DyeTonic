using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace DyeTonic
{
    public class HPbarUpdater : MonoBehaviour
    {
        private Slider HPSlider;
        [SerializeField] private SongManager _songManager;

        private void Start()
        {
            HPSlider = gameObject.GetComponent<Slider>();
        }


        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnNoteMiss += UpdateUI;
            Note.OnNoteSelfDestroy += UpdateUI;
        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnNoteMiss -= UpdateUI;
            Note.OnNoteSelfDestroy -= UpdateUI;
        }

        void UpdateUI()
        {
            //reduce HP
            if (_songManager.HP - 5 < 0)
                _songManager.HP = 0;
            else
                _songManager.HP -= 5;

            HPSlider.value = (float)_songManager.HP / 100;

        }
    }
}
