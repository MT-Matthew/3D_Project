using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    Slider slider;
    public Text hydrationCounter;

    private float currentHydration, maxHydration;
 

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = PlayerState.Instance.currentHydrationPercent;
        maxHydration = PlayerState.Instance.maxHydrationPercent;

        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;
 
        hydrationCounter.text = currentHydration + "%";
    }
}
