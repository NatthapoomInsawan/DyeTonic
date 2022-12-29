using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DyeTonic
{
    public class HitTrigger : MonoBehaviour
    {
        bool wasPressed;
        LongNote hitLongNote;

        private void Update()
        {
            //Draw line when held input
            if (wasPressed)
                Debug.DrawLine(transform.position + new Vector3(0, 0, -2.5f), transform.position + new Vector3(0, 0, 2.5f), Color.red);

        }

        public void Track1 (InputAction.CallbackContext context) => CalculateScore(context);

        public void Track2 (InputAction.CallbackContext context) => CalculateScore(context);

        public void Track3 (InputAction.CallbackContext context) => CalculateScore(context);

        public void Track4 (InputAction.CallbackContext context) => CalculateScore(context);

        private void CalculateScore (InputAction.CallbackContext context)
        {

            Ray ray = new Ray(transform.position + new Vector3(0, 0, -2.5f), transform.forward);

            RaycastHit hit;

            if (context.action.WasPressedThisFrame())
            {
                wasPressed = true;
                if (Physics.Raycast(ray, out hit, 5))
                {
                    LongNote longnoteComponent = hit.transform.GetComponent<LongNote>();

                    Debug.Log(hit.transform.gameObject.name);
                    Debug.Log(wasPressed);

                    if (longnoteComponent != null)
                    {
                        hitLongNote = longnoteComponent;
                    }
                    else
                    {
                        if (context.action.WasPressedThisFrame())
                            Destroy(hit.transform.gameObject);
                    }

                }
            }

            if (context.action.WasReleasedThisFrame())
            {
                wasPressed = false;
                if (hitLongNote != null)
                {
                    Destroy(hitLongNote.gameObject);
                }
            }

            if (context.action.actionMap.name == "Player2")
            {

            }
            else
            {

            }

        }

    }
}
