using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public bool playerInRange;
 

    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }

        if (cookingTimer <= 0 && isCooking)
        {
            isCooking = false;
            readyFood = GetCookedFood(foodBeingCooked);
        }
    }

    private string GetCookedFood(CookableFood food)
    {
        return food.cookedFoodName;
    }

    public CookableFood foodBeingCooked;
    public string readyFood;
    public bool isCooking;
    public float cookingTimer;
    public GameObject fire;


    public void OpenUI()
    {
        CampfireUIManager.Instance.OpenUI();
        CampfireUIManager.Instance.selectedCampfire = this;

        if (readyFood != "")
        {
            GameObject rf = Instantiate(Resources.Load<GameObject>(readyFood),
                CampfireUIManager.Instance.foodSlot.transform.position,
                CampfireUIManager.Instance.foodSlot.transform.rotation);

            rf .transform.SetParent(CampfireUIManager.Instance.foodSlot.transform);

            // Đặt vị trí, góc quay và tỷ lệ mới cho vật phẩm
            rf.transform.localPosition = Vector3.zero;
            rf.transform.localRotation = Quaternion.identity;
            rf.transform.localScale = Vector3.one;

            readyFood = "";
        }
    }

    public void StartCooking(InventoryItem food)
    {
        foodBeingCooked = ConvertIntoCookable(food);
        isCooking = true;
        cookingTimer = TimeToCookFood(foodBeingCooked);
    }

    private CookableFood ConvertIntoCookable(InventoryItem food)
    {
        foreach (CookableFood cookable in CampfireUIManager.Instance.cookingData.validFoods)
        {
            if (cookable.name == food.thisName)
            {
                return cookable;
            }
        }

        return new CookableFood();
    }

    private float TimeToCookFood(CookableFood food)
    {
        return food.timeToCook;
    }
}
