using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DyeTonic
{
    public class AudioPlayer : MonoBehaviour
    {
        private static AudioPlayer instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        [SerializeField] private List<AudioChannelSO> audioChannels;

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

        public void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup)
        {

            GameObject instantiateAudio = new GameObject("Instantiated Audio");
            AudioSource audioSource = (AudioSource)instantiateAudio.AddComponent(typeof(AudioSource));

            audioSource.clip = _audioClip;
            audioSource.outputAudioMixerGroup = _audioMixerGroup;
            audioSource.Play();

            Destroy(instantiateAudio, audioSource.clip.length);
        }

    }
}
