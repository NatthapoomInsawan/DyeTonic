using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class PersistentDataLoader : MonoBehaviour
    {
        [SerializeField] private GameSettings _settings;

        private void Awake()
        {
            _settings.LoadData();

            DontDestroyOnLoad(this);
        }
    }
}
