using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DyeTonic
{
    public class HitTrigger : MonoBehaviour
    {
        public void Track1 (InputAction.CallbackContext context)
        {
            CalculateScore(context);
        }

        public void Track2 (InputAction.CallbackContext context)
        {
            CalculateScore(context);
        }

        public void Track3 (InputAction.CallbackContext context)
        {
            CalculateScore(context);
        }

        public void Track4 (InputAction.CallbackContext context)
        {
            CalculateScore(context);
        }

        private void CalculateScore (InputAction.CallbackContext context)
        {

            Ray ray = new Ray(transform.position + new Vector3(0, 0, -2.5f), transform.forward);

            RaycastHit hit;

            Debug.DrawLine(transform.position + new Vector3(0, 0, -2.5f), transform.position + new Vector3(0, 0, 2.5f), Color.red, 2f);

            if (Physics.Raycast(ray, out hit, 5))
            {
                LongNote longnoteComponent = hit.transform.GetComponent<LongNote>();

                if (longnoteComponent != null)
                {
                    
                }
                else
                {
                    
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
