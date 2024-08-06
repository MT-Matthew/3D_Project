using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    Slider slider;
    public Text oxygenCounter;

    private float currentOxygen, maxOxygen;
 

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentOxygen = PlayerState.Instance.currentOxygenPercent;
        maxOxygen = PlayerState.Instance.maxOxygenPercent;

        float fillValue = currentOxygen / maxOxygen;
        slider.value = fillValue;
 
        oxygenCounter.text = currentOxygen + "%";
    }
}
