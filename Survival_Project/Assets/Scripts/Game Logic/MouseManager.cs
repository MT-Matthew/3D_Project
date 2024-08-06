using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            MenuManager.Instance.isMenuOpen == false &&
            StorageManager.Instance.storageUIOpen == false) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerState.Instance.cameraBrain.SetActive(true);

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerState.Instance.cameraBrain.SetActive(false);

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
    }
}
