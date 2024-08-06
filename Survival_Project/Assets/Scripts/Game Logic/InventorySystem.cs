using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
 
   public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;


    public bool isOpen;
    public bool isFull;
    public GameObject cameraBrain;

    // Pickup Popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> itemsPickedup;

    public GameObject ItemInfoUI;
 
    public bool isStack;
 
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
        isOpen = false;
        isFull = false;

        PopulateSlotList();
        ReCalculateList();

        Cursor.visible = false;
    }
 
 
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen && ConstructionManager.Instance.inConstructionMode == false  && PlacementSystem.Instance.inPlacementMode == false)
        {
            OpenUI();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            CloseUI();
        }

        
    }

    public void OpenUI()
    {
        inventoryScreenUI.SetActive(true);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        PlayerState.Instance.cameraBrain.SetActive(false);
    }

    public void CloseUI()
    {
        inventoryScreenUI.SetActive(false);
        isOpen = false;    

        if (CraftingSystem.Instance.isOpen == false &&
            CampfireUIManager.Instance.isUiOpen == false &&
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

    void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Inventory Slot"))
            {
                slotList.Add(child.gameObject);
            }
            
        }

    }


    public bool CheckSlotAvailable(int emtyNeeded)
    {
        int emtySlot = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {
                emtySlot += 1;
            }
        }
        if (emtySlot >= emtyNeeded)
        {
            isFull = false;
            return true;
        }
        else
        {
            isFull = true;
            return false;
        }
    }


    public void CheckIfEachSlotIsFull()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.GetComponent<ItemSlot>().currentQuantity >= slot.GetComponent<ItemSlot>().slotMaxQuantity)
            {
                slot.GetComponent<ItemSlot>().isFull = true;
            }
            else
            {
                slot.GetComponent<ItemSlot>().isFull = false;
            }
        }
    }

    GameObject FindNextEmtySlot(String name)
    {
        isStack = false;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                if (slot.GetComponent<ItemSlot>().isFull == false && (slot.transform.GetChild(0).name == name || slot.transform.GetChild(0).name == name + "(Clone)"))
                {
                    isStack = true;
                    return slot;
                }
            }

            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public void AddToInventory(string itemName)
    {   
        if (SaveManager.Instance)
        {
            if (SaveManager.Instance.isLoading == false)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
            }
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
        }
        
        
        whatSlotToEquip = FindNextEmtySlot(itemName);

        if (isStack)
        {
            whatSlotToEquip.GetComponent<ItemSlot>().currentQuantity += 1;
            TriggerPickupPopup(itemName, Resources.Load<GameObject>(itemName).GetComponent<Image>().sprite);
        }
        else
        {
            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            // Đặt vị trí, góc quay và tỷ lệ mới cho vật phẩm
            itemToAdd.transform.localPosition = Vector3.zero;
            itemToAdd.transform.localRotation = Quaternion.identity;
            itemToAdd.transform.localScale = Vector3.one;
            whatSlotToEquip.GetComponent<ItemSlot>().currentQuantity = 1;

            itemList.Add(itemName);
            TriggerPickupPopup(itemName, itemToAdd.GetComponent<Image>().sprite);
        }

        

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        
        for (int i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void ReCalculateList()
    {
        itemList.Clear();
        inventoryDataList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str = "(Clone)";
                string result = name.Replace(str, "");

                itemList.Add(result);
            }

            if (slot.GetComponent<ItemSlot>())
            {
                var targetSlot = slot.GetComponent<ItemSlot>();
                GameObject prefab = null;
                int maxQuantity = 0;
                int currentStack = 0;

                if (slot.transform.childCount > 0)
                {
                    string name = slot.transform.GetChild(0).name;
                    string str = "(Clone)";
                    string result = name.Replace(str, "");

                    prefab = Resources.Load<GameObject>(result);
                    maxQuantity = slot.transform.GetChild(0).GetComponent<InventoryItem>().maxQuantity;
                    targetSlot.slotMaxQuantity = maxQuantity;
                    currentStack = targetSlot.currentQuantity;
                }
                else
                {
                    prefab = null;
                    maxQuantity = 64;
                    currentStack = 1;
                }
                

                InventoryData tempData = new InventoryData();
                tempData.slotNumber = targetSlot.slotNumber;
                tempData.itemPrefab = prefab;
                tempData.itemMaxQuantity = maxQuantity;
                tempData.currentQuantity = currentStack;

                inventoryDataList.Add(tempData);
            }
        }
        CheckIfEachSlotIsFull();
    }

    void TriggerPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        pickupAlert.SetActive(false);
    }

    public List<InventoryData> inventoryDataList;

 
}


[System.Serializable]
public class InventoryData
{
    public int slotNumber;
    public GameObject itemPrefab;
    public int itemMaxQuantity;
    public int currentQuantity;
}