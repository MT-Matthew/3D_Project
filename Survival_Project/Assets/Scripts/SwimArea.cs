using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimArea : MonoBehaviour
{
    public GameObject oxygenBar;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            other.GetComponentInParent<PlayerMovement>().isSwiming = true;
        }

        if (other.CompareTag("Top"))
        {
            other.GetComponentInParent<PlayerMovement>().isUnderWater = true;
            
            oxygenBar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            other.GetComponentInParent<PlayerMovement>().isSwiming = false;
        }

        if (other.CompareTag("Top"))
        {
            other.GetComponentInParent<PlayerMovement>().isUnderWater = false;

            oxygenBar.SetActive(false);
            PlayerState.Instance.currentOxygenPercent = PlayerState.Instance.maxOxygenPercent;
        }
    }


}
