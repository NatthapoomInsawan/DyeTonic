using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace DyeTonic
{
    public class SettingSceneManager : MonoBehaviour
    {

        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private AudioMixer _audioMixer;

        [Header("Slider reference")]
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;

        // Start is called before the first frame update
        void Start()
        {
            _masterSlider.value = _gameSettings.masterVolume;
            _musicSlider.value = _gameSettings.musicVolume;
            _SFXSlider.value = _gameSettings.SFXVolume;

            _audioMixer.SetFloat("Master", _gameSettings.masterVolume);
            _audioMixer.SetFloat("Music", _gameSettings.musicVolume);
            _audioMixer.SetFloat("SFX", _gameSettings.SFXVolume);
        }

        public void OnMasterSliderChange (Slider _slider)
        {
            _gameSettings.masterVolume = _slider.value;
            _audioMixer.SetFloat("Master", _gameSettings.masterVolume);
        }

        public void OnMusicSliderChange (Slider _slider)
        {
            _gameSettings.musicVolume = _slider.value;
            _audioMixer.SetFloat("Music", _gameSettings.musicVolume);
        }

        public void OnSFXSliderChange(Slider _slider)
        {
            _gameSettings.SFXVolume = _slider.value;
            _audioMixer.SetFloat("SFX", _gameSettings.SFXVolume);
        }
    }
}
