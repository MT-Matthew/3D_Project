using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
 
    // [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
 
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    public static Transform startParent;
 
 
 
    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
 
    }
 
 
    public void OnBeginDrag(PointerEventData eventData)
    {
 
        // Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
 
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta;
 
    }
 
 
 
    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
        itemBeingDragged = null;
 
        if (/*transform.parent == startParent ||*/ transform.parent == transform.root)
        {
            // hide the icon of the item at this poin
            tempItemReference.SetActive(false);

            AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();

            dialogManager.ShowDialog("Do you want to drop this item?", (responce)=>
            {
                if (responce)
                {
                    DropItemIntoTheWorld(tempItemReference);
                }
                else
                {
                    transform.position = startPosition;
                    transform.SetParent(startParent);
                    rectTransform.localPosition = Vector3.zero;
                    tempItemReference.SetActive(true);
                }
            });


            
        }
 
        // Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        // get clean name
        string cleanName = tempItemReference.name.Split(new string[] {"(Clone)"}, StringSplitOptions.None) [0];

        // instantiate item
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("Drop Spawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        // set instantiated item to be the child of [Items] object
        var itemObject = FindObjectOfType<EnvironmentManager>().gameObject.transform.Find("[Items]");
        item.transform.SetParent(itemObject.transform);

        // delete item from inventory
        DestroyImmediate(tempItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

    
    }
}