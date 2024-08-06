using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool onTarget = false;
    public string ItemName;
    public float detectionRange = 10f;


    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && onTarget && SelectionManager.Instance.canPickup)
        {
            if (InventorySystem.Instance.CheckSlotAvailable(1)) // InventorySystem.Instance.CheckSlotAvailable(0) == false
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }
 
    public string GetItemName()
    {
        return ItemName;
    }
}