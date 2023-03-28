using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Audio Channel")]
    public class AudioChannelSO : ScriptableObject
    {
        public event Action<AudioClip, AudioMixerGroup> OnAudioPlayRequest;
        public event Action<AudioClip, AudioMixerGroup, float> OnAudioPlayRequestWithTime;
        [field:SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }

        public void RaisePlayRequest(AudioClip audioClip)
        {
            OnAudioPlayRequest?.Invoke(audioClip, AudioMixerGroup);
        }

        public void RaisePlayRequest(AudioClip audioClip, float time)
        {
            OnAudioPlayRequestWithTime?.Invoke(audioClip, AudioMixerGroup, time);
        }

    }
}
