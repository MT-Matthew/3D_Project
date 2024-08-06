using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int maxQuantity;

    // --- Is this item trashable --- //
    public bool isTrashable;
 
    // --- Item Info UI --- //
    private GameObject itemInfoUI;
 
    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;
 
    public string thisName, thisDescription, thisFunctionality;
 
    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;
 
    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;


    //--- Equipoing ---//
    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

    public bool isSelected;

    public bool isUseable;
 
 
    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("Item Name").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("Item Description").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("Item Functionality").GetComponent<Text>();
    }
    
    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }
    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }
 
    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }
 
    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }

            if (isEquippable == true && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUseable)
            {
                gameObject.SetActive(false);
                UseItem();
            }
        }

        
    }
 
    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }

            // if (isUseable && itemPendingToBeUsed == gameObject)
            // {
            //     DestroyImmediate(gameObject);
            //     InventorySystem.Instance.ReCalculateList();
            //     CraftingSystem.Instance.RefreshNeededItems();
            // }
        }
    }
 
    private void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);
 
        healthEffectCalculation(healthEffect);
 
        caloriesEffectCalculation(caloriesEffect);
 
        hydrationEffectCalculation(hydrationEffect);
 
    }
 
 
    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //
 
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;
 
        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }
 
 
    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // --- Calories --- //
 
        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;
 
        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.setCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }
 
 
    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        // --- Hydration --- //
 
        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;
 
        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }

    void UseItem()
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);
        InventorySystem.Instance.cameraBrain.SetActive(true);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        CraftingSystem.Instance.refineScreenUI.SetActive(false);
        CraftingSystem.Instance.constructionScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled =true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Foundation":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall(Clone)":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Wall":
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Chest(Clone)":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("ChestModel");
                break;
            case "Chest":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("ChestModel");
                break;
            case "Campfire(Clone)":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("CampfireModel");
                break;
            case "Campfire":
                PlacementSystem.Instance.inventoryItemToDestory = gameObject;
                PlacementSystem.Instance.ActivatePlacementMode("CampfireModel");
                break;
            default:
                break;
        }


    }
 
 
}