using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CampfireUIManager : MonoBehaviour
{
    public static CampfireUIManager Instance {get; set;}

    void Awake()
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

    public GameObject foodSlot;
    public GameObject fuelSlot;
    public Button exitButton;
    public Button cookButton;

    public GameObject campfirePanel;
    public bool isUiOpen;

    public Campfire selectedCampfire;
    public CookingData cookingData;

    void Update()
    {
        if (FuelAndFoodAreValid())
        {
            cookButton.interactable = true;
        }
        else
        {
            cookButton.interactable = false;
        }
    }

    private bool FuelAndFoodAreValid()
    {
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();

        if (fuel != null && food != null)
        {
            if (cookingData.validFuels.Contains(fuel.thisName)
                && cookingData.validFoods.Any(cookableFood => cookableFood.name == food.thisName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;

    }

    public void CookButtonPressed()
    {
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();

        selectedCampfire.StartCooking(food);

        Destroy(food.gameObject);
        Destroy(fuel.gameObject);

        CloseUI();
    }

    public void OpenUI()
    {
        campfirePanel.SetActive(true);
        isUiOpen = true;
        InventorySystem.Instance.OpenUI();
 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
 
        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        PlayerState.Instance.cameraBrain.SetActive(false);
    }

    public void CloseUI()
    {
        campfirePanel.SetActive(false);
        isUiOpen = false;
 
        if (CraftingSystem.Instance.isOpen == false &&
            InventorySystem.Instance.isOpen == false &&
            StorageManager.Instance.storageUIOpen == false &&
            MenuManager.Instance.isMenuOpen == false)
            {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            PlayerState.Instance.cameraBrain.SetActive(true);
        }
    }

}
