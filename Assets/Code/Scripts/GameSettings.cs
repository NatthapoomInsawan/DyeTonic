using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DyeTonic
{
    [CreateAssetMenu(menuName = "Scriptable Object/Game Settings")]
    [System.Serializable]
    public class GameSettings : ScriptableObject
    {
        public float masterVolume = 20;
        public float musicVolume = 20;
        public float SFXVolume = 20;

        public int progress = 0;

        public void SaveData()
        {
            string path = Path.Combine(Application.persistentDataPath, "settings.json");

            try
            {
                if (File.Exists(path))
                {
                    Debug.Log("file exist!");
                    File.Delete(path);
                }
                FileStream fileStream = File.Create(path);
                fileStream.Close();
                File.WriteAllText(path, JsonUtility.ToJson(this));
                Debug.Log("saved at" + path);
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to save data due to:" + e.Message);
            }

        }

        public void LoadData()
        {
            string path = Path.Combine(Application.persistentDataPath, "settings.json");

            if (!File.Exists(path))
            {
                Debug.LogWarning("file does not exist!");
                return;
            }

            try
            {
                GameSettings loadedSettings = (GameSettings)CreateInstance(typeof(GameSettings));
                JsonUtility.FromJsonOverwrite(File.ReadAllText(path), loadedSettings);

                musicVolume = loadedSettings.musicVolume;
                SFXVolume = loadedSettings.SFXVolume;
                progress = loadedSettings.progress;
                Debug.Log("loaded settings");

            }
            catch (Exception e)
            {
                Debug.LogError("failed to load data due to:" + e.Message);
            }

        }
    }
}
