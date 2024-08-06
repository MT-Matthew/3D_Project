using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    public bool swingWait;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            MenuManager.Instance.isMenuOpen == false &&
            SelectionManager.Instance.handIsVisible == false &&
            swingWait == false &&
            ConstructionManager.Instance.inConstructionMode == false)
        {
            swingWait = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
            animator.SetTrigger("hit");
            StartCoroutine(NewSwingDelay());
        }
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }
}
