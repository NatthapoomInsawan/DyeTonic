using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Game Settings")]
    [System.Serializable]
    public class GameSettings : ScriptableObject
    {
        public float musicVolume = 1;
        public float SFXVolume = 1;

        public int progress = 0;
    }
}
