using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DyeTonic
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] private List<AudioChannelSO> audioChannels;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest += PlayAudio;
            }
        }

        private void OnDisable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest -= PlayAudio;
            }
        }

        public void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, float _timeToStart)
        {
            audioSource.clip = _audioClip;
            audioSource.outputAudioMixerGroup = _audioMixerGroup;
            audioSource.time = _timeToStart;
            audioSource.Play();
        }

    }
}
