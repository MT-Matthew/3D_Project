using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
 
 
public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int slotNumber;
    public int slotMaxQuantity;
    public int currentQuantity;
    public bool isFull;


    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0 )
            {
                return transform.GetChild(0).gameObject;
            }
 
            return null;
        }
    }
 
 
 
 
 
 
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
 
        //if there is not item already then set our item.
        if (!Item)
        {

            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);
 
            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector3(0, 0, 0);

            // Debug.Log("new: " + transform);
            // Debug.Log("old: " + DragDrop.startParent);


            transform.GetComponent<ItemSlot>().currentQuantity = DragDrop.startParent.GetComponent<ItemSlot>().currentQuantity;

            DragDrop.startParent.GetComponent<ItemSlot>().currentQuantity = 0;
            DragDrop.startParent.GetComponent<ItemSlot>().slotMaxQuantity = 64;


            if (transform.CompareTag("Quick Slot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();
            }

            if (transform.CompareTag("Quick Slot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else if (Item && Item.transform.parent.GetComponent<ItemSlot>().isFull == false && transform.GetChild(0).name == DragDrop.itemBeingDragged.name + "(Clone)")
        {
            int aQuantity = DragDrop.itemBeingDragged.transform.parent.GetComponent<ItemSlot>().currentQuantity;
            int bQuantity = Item.GetComponent<ItemSlot>().currentQuantity;
            int availableQuantity = Item.GetComponent<ItemSlot>().slotMaxQuantity - bQuantity;

            if (availableQuantity < aQuantity)
            {
                DragDrop.itemBeingDragged.transform.parent.GetComponent<ItemSlot>().currentQuantity = aQuantity - availableQuantity;
                Item.GetComponent<ItemSlot>().currentQuantity = Item.GetComponent<ItemSlot>().slotMaxQuantity;
            }
            else
            {
                DragDrop.itemBeingDragged.transform.parent.GetComponent<ItemSlot>().slotMaxQuantity = 0;
                DragDrop.itemBeingDragged.transform.parent.GetComponent<ItemSlot>().currentQuantity = 0;
                Item.GetComponent<ItemSlot>().currentQuantity = aQuantity + bQuantity;
            }
        }
        else if (Item && (Item.transform.parent.GetComponent<ItemSlot>().isFull == true || transform.GetChild(0).name != DragDrop.itemBeingDragged.name + "(Clone)"))
        {
            Transform posA = DragDrop.startParent;
            Transform posB = transform;

            DragDrop.itemBeingDragged.transform.SetParent(posB);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector3(0, 0, 0);

            transform.GetChild(0).SetParent(posA);
            DragDrop.startParent.GetChild(0).localPosition = new Vector3(0, 0, 0);

        }

        InventorySystem.Instance.ReCalculateList();
 
 
    }
}