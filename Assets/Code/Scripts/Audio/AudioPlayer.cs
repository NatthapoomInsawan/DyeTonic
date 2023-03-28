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

        private static GameObject currentSong;

        private void OnEnable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest += PlayAudio;
                channel.OnAudioPlayRequestWithTime += PlayAudio;
            }
        }

        private void OnDisable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest -= PlayAudio;
                channel.OnAudioPlayRequestWithTime += PlayAudio;
            }
        }

        public void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup)
        {
            PlayAudio(_audioClip, _audioMixerGroup, 0.0f);
        }

        public void PlayAudio(AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, float time)
        {
            GameObject instantiateAudio = new GameObject("Instantiated Audio");
            AudioSource audioSource = (AudioSource)instantiateAudio.AddComponent(typeof(AudioSource));

            //if audio is music
            if (_audioMixerGroup == audioChannels[1].AudioMixerGroup)
            {
                Destroy(currentSong);
                currentSong = instantiateAudio;
                Debug.Log("stop old song");
            }

            audioSource.clip = _audioClip;
            audioSource.outputAudioMixerGroup = _audioMixerGroup;
            audioSource.time = time;
            audioSource.Play();

            Destroy(instantiateAudio, audioSource.clip.length);
        }

        public static void StopCurrentMusic()
        {
            Destroy(currentSong);
        }

    }
}
