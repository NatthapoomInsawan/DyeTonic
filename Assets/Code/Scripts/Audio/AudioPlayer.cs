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

        [SerializeField] private AudioClip startBGM;

        [SerializeField] private List<AudioChannelSO> audioChannels;

        [SerializeField] private GameObject currentSong;

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

        private void Start()
        {
            if (audioChannels != null)
                PlayAudio(startBGM, audioChannels[1].AudioMixerGroup);
        }

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
                channel.OnAudioPlayRequestWithTime -= PlayAudio;
            }
        }

        public void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup)
        {
            PlayAudio(_audioClip, _audioMixerGroup, 0.0f);
        }

        public void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, float time)
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
            DontDestroyOnLoad(instantiateAudio);
        }

        public void StopCurrentMusic()
        {
            Destroy(currentSong);
        }

    }
}
