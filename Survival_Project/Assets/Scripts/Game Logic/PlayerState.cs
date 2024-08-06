using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // Player Health //
    public float currentHealth;
    public float maxHealth;

    // Player Calories //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;
    public GameObject cameraBody;
    public GameObject cameraBrain;
    float timer = 1;


    // Player Hydration //
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydration;

    // Player Hydration //
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 1;
    public float oxygenTimer = 0;
    public float decreaseInterval = 1;

    public float outOfAirDamagePerSecond = 5;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        currentOxygenPercent = maxOxygenPercent;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(2);
        }
    }


    void Update()
    {
        if (playerBody.GetComponent<PlayerMovement>().isUnderWater)
        {
            oxygenTimer += Time.deltaTime;
            if (oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;
        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            currentCalories -= 1;
            timer = 1;
        }
    }

    private void DecreaseOxygen()
    {
        currentOxygenPercent -= oxygenDecreasedPerSecond * decreaseInterval;

        if (currentOxygenPercent <= 0)
        {
            currentOxygenPercent = 0;
            setHealth(currentHealth - outOfAirDamagePerSecond);
        }
    }

    public void setHealth(float value){
        currentHealth = value;
    }

    public void setCalories(float value){
        currentCalories = value;
    }

    public void setHydration(float value){
        currentHydrationPercent = value;
    }
}
