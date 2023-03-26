using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DyeTonic
{
    public class LevelSceneManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private GameSettings _gameSettings;

        [Header("Level color section")]
        [SerializeField] private List<Transform> colorSections;

        [Header("Level select button")]
        [SerializeField] private List<Button> levelSelectButtons;

        // Start is called before the first frame update
        void Start()
        {
            SetupColorSections();
            SetUpSelectButtons();
        }

        private void SetupColorSections()
        {
            foreach (var section in colorSections)
            {
                if (_gameSettings.progress > colorSections.IndexOf(section))
                    section.gameObject.SetActive(true);
            }
        }

        private void SetUpSelectButtons()
        {
            foreach (var button in levelSelectButtons)
            {
                if (_gameSettings.progress >= levelSelectButtons.IndexOf(button))
                {
                    button.interactable = true;
                }
            }
        }
    }
}
