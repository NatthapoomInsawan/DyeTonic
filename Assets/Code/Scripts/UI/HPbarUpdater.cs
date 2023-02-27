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
        [SerializeField] private int damageValue = 5;
        [SerializeField] private int healValue = 1;

        private void Start()
        {
            HPSlider = gameObject.GetComponent<Slider>();
        }


        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnNoteMiss += DamageHP;
            HitTrigger.OnNoteHit += HealHP;
        }

        private void OnDisable()
        {
            //unsubscribe event
            HitTrigger.OnNoteMiss -= DamageHP;
            HitTrigger.OnNoteHit -= HealHP;
        }

        void UpdateUI()
        {
            HPSlider.value = (float)_songManager.HP / 100;
        }

        void DamageHP()
        {
            //reduce HP
            if (_songManager.HP - damageValue < 0)
                _songManager.HP = 0;
            else
                _songManager.HP -= damageValue;

            UpdateUI();
        }

        void HealHP()
        {
            //Heal HP
            if (_songManager.HP + healValue > 100)
                _songManager.HP = 100;
            else
                _songManager.HP += healValue;

            UpdateUI();
        }
    }
}
