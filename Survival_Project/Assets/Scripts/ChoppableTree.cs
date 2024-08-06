using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public float detectionRange = 10f;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator animator;

    public float caloriesSpentChoppingWood = 20;

    private void Start()
    {
        treeHealth = treeMaxHealth; 
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void Update()
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


        if (canBeChopped)
        {
            GlobalState.Instance.resourcesHealth = treeHealth;
            GlobalState.Instance.resourcesMaxHealth = treeMaxHealth;
        }
    }


    public void GetHit()
    {
        animator.SetTrigger("shake");
        treeHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

        if (treeHealth <= 0)
        {
            TreeIsDead();
        }

    }

    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;
        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Chopped Tree"), new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));
        brokenTree.transform.SetParent(transform.parent.transform.parent.transform.parent);

        brokenTree.GetComponent<RegrowTree>().dayOfRegrowth = TimeManager.Instance.dayInGame + 2;
    }

}
