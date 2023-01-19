using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DyeTonic
{
    public class NoteSelector : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _noteInformationText;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Note noteComponent = hit.transform.GetComponent<Note>();

                    if (noteComponent != null)
                        _noteInformationText.text = "Note: " + noteComponent.gameObject.name + "\n"
                                                    + "Beat: " + noteComponent.NoteData.beat + "\n"
                                                    + "Track: " + noteComponent.NoteData.track + "\n"
                                                    + "EndBeat: " + noteComponent.NoteData.endBeat;
                }
            }
        
        }
    }
}
