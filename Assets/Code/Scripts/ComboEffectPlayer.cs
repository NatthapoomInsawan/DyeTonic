using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class ComboEffectPlayer : MonoBehaviour
    {
        [SerializeField] private SongManager _songManager;
        [SerializeField] private int comboToPlayEffect;
        private int counter;

        [Header("Effect Particle")]
        [SerializeField] ParticleSystem leftScreenParticle;
        [SerializeField] ParticleSystem rightScreenParticle;

        private void OnEnable()
        {
            //subscribe event
            HitTrigger.OnScoreUpdate += PlayEffect;
            HitTrigger.OnNoteMiss += NoteMiss;
        }

        private void OnDisable()
        {
            HitTrigger.OnScoreUpdate -= PlayEffect;
            HitTrigger.OnNoteMiss -= NoteMiss;
        }

        private void PlayEffect()
        {
            if (counter < comboToPlayEffect)
                counter++;
            else
                counter = 0;

            if (counter == comboToPlayEffect)
            {
                leftScreenParticle.Play();
                rightScreenParticle.Play();
            }
        }

        private void NoteMiss()
        {
            counter = 0;
        }

    }
}
