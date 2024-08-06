using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    Slider slider;
    public Text caloriesCounter;
    public GameObject playerState;

    private float currentCalories, maxCalories;
 

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCalories = PlayerState.Instance.currentCalories;
        maxCalories = PlayerState.Instance.maxCalories;

        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;

        caloriesCounter.text = currentCalories + "/" + maxCalories;
    }
}
