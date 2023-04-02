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

        [SerializeField] private GameObject currentSong;

        [SerializeField] private AudioMixerGroup musicAudioMixer;

        [SerializeField] private List<AudioChannelSO> audioChannels;

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
                PlayBGMAudio( startBGM, musicAudioMixer, true);
        }

        private void OnEnable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest += PlayAudio;
                channel.OnAudioPlayRequestWithTime += PlayAudio;
                channel.OnBGMPlayRequest += PlayBGMAudio;
            }
        }

        private void OnDisable()
        {
            foreach (AudioChannelSO channel in audioChannels)
            {
                channel.OnAudioPlayRequest -= PlayAudio;
                channel.OnAudioPlayRequestWithTime -= PlayAudio;
                channel.OnBGMPlayRequest -= PlayBGMAudio;
            }
        }

        private void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup)
        {
            PlayAudio(_audioClip, _audioMixerGroup, 0.0f);
        }

        private void PlayAudio(AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, float time)
        {
            PlayAudio(_audioClip, _audioMixerGroup, time, false);
        }

        private void PlayBGMAudio(AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, bool isLoop)
        {
            if (_audioMixerGroup == musicAudioMixer)
                PlayAudio(_audioClip, _audioMixerGroup, 0.0f, isLoop);
            else
                Debug.LogWarning("Wrong audioMixer group requested.");
        }

        private void PlayAudio (AudioClip _audioClip, AudioMixerGroup _audioMixerGroup, float time, bool isLoop)
        {
            GameObject instantiateAudio = new GameObject("Instantiated Audio");
            AudioSource audioSource = (AudioSource)instantiateAudio.AddComponent(typeof(AudioSource));

            //if audio is music
            if (_audioMixerGroup == musicAudioMixer)
            {
                Destroy(currentSong);
                currentSong = instantiateAudio;
                Debug.Log("stop old song");
            }

            audioSource.clip = _audioClip;
            audioSource.outputAudioMixerGroup = _audioMixerGroup;
            audioSource.time = time;
            audioSource.Play();

            //if loop
            if (isLoop)
                audioSource.loop = true;
            else
                Destroy(instantiateAudio, audioSource.clip.length);

            DontDestroyOnLoad(instantiateAudio);
        }

        public void StopCurrentMusic()
        {
            Destroy(currentSong);
        }

    }
}
