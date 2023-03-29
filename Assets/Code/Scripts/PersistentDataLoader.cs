using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DyeTonic
{
    public class PersistentDataLoader : MonoBehaviour
    {
        [SerializeField] private GameSettings _settings;

        private PersistentDataLoader instance;

        private void Awake()
        {
            _settings.LoadData();

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
    }
}
