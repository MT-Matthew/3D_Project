using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }
 
    // -- UI -- //
    public GameObject quickSlotsPanel;
 
    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;
    public GameObject selectedItemModel;

    public bool isPlayerHoldingSeed()
    {
        if (selectedItemModel != null)
        {
            switch (selectedItemModel.gameObject.name)
            {
                case "Hand_Model(Clone)":
                    return true;
                case "Hand_Model":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }
 
   
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
 
 
    private void Start()
    {
        PopulateSlotList();
    }
 
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("Quick Slot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }

    public void SelectQuickSlot(int number)
    {
        if(CheckIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                // Unselect previously selected item
                if(selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquippedModel(selectedItem);


                // Changing Color
                ChangeColor();

                Text toBeChange = numbersHolder.transform.Find("Number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChange.color = Color.white;
            }
            else if(selectedNumber == number) // Are trying to select the same slot
            {
                selectedNumber = -1; //null

                // Unselect previously selected item
                if(selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }


                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                // Changing Color
                ChangeColor();

                selectedItem = null;

            }

        }
    }

    public void ChangeColor()
    {
        foreach (Transform child in numbersHolder.transform)
        {
            child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        }
    }
 
    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");


        // Đặt vị trí, góc quay và tỷ lệ mới cho vật phẩm
        itemToEquip.transform.localPosition = Vector3.zero;
        itemToEquip.transform.localRotation = Quaternion.identity;
        itemToEquip.transform.localScale = Vector3.one;
 
        InventorySystem.Instance.ReCalculateList();
 
    }

    bool CheckIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber -1].transform.childCount >0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }
 
 
    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }
 
    public bool CheckIfFull()
    {
 
        int counter = 0;
 
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
 
        if (counter == quickSlotsList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }
        
        string selectedItemName = selectedItem.name.Replace("(Clone)", "");

        // selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(1f, 0f, 0f), Quaternion.Euler(25f, 98f, 8f));

        selectedItemModel = Instantiate(Resources.Load<GameObject>(CalculateItemModel(selectedItemName)));

        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private string CalculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                return "Axe_Model";
            case "Hammer":
                return "Hammer_Model";
            case "Tomato Seed":
                return "Hand_Model";
            case "Pumpkin Seed":
                return "Hand_Model";
            case "Watering Can":
                return "WateringCan_Model";
            case "Fishing Rod":
                return "FishingRod_Model";
            default:
                return null;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }

    internal bool IsThereASwingLock()
    {
        if (selectedItemModel && selectedItemModel.GetComponent<EquipableItem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHoldingWateringCan()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.GetComponent<InventoryItem>().thisName)
            {
                case "Watering Can":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }
}