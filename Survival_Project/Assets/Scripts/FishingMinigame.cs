using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform catcherTransform;

    public bool isFishOverlapping;

    public Slider successSlider;
    float successIncrement = 20;
    float failDecrement = 12;
    float successThreshold = 100;
    float failThreshold = -100;
    float successCounter = 0;

    void Update()
    {
        if (CheckOverlapping(fishTransform, catcherTransform))
        {
            isFishOverlapping = true;
        }
        else
        {
            isFishOverlapping = false;
        }

        OverlappingCalculation();
    }

    private void OverlappingCalculation()
    {
        if (isFishOverlapping)
        {
            successCounter += successIncrement * Time.deltaTime;
        }
        else
        {
            successCounter -= failDecrement * Time.deltaTime;
        }

        successCounter = Mathf.Clamp(successCounter, failThreshold, successThreshold);
        successSlider.value = successCounter;

        if (successCounter >= successThreshold)
        {
            FishingSystem.Instance.EndMinigame(true);
            successCounter = 0;
            successSlider.value = 0;
        }
        else if(successCounter <= failThreshold)
        {
            FishingSystem.Instance.EndMinigame(false);
            successCounter = 0;
            successSlider.value = 0;
        }

    }

    private bool CheckOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = new Rect(rect1.position.x, rect1.position.y, rect1.rect.width, rect1.rect.height);
        Rect r2 = new Rect(rect2.position.x, rect2.position.y, rect2.rect.width, rect2.rect.height);

        return r1.Overlaps(r2);
    }
}
