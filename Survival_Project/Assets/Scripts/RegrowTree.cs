using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowTree : MonoBehaviour
{
    public int dayOfRegrowth;

    public bool growthLock = false;

    void Update()
    {
        if (TimeManager.Instance.dayInGame == dayOfRegrowth && growthLock == false)
        {
            growthLock = true;
            RegrowNewTree();
        }
    }

    private void RegrowNewTree()
    {
        DisplaceLogs();
        
        gameObject.SetActive(false);
        GameObject newTree = Instantiate(Resources.Load<GameObject>("Tree_Parent"),
            new Vector3(transform.position.x, transform.position.y, transform.position.z),
            Quaternion.Euler(0, 0, 0));

        newTree.transform.SetParent(transform.parent);

        Destroy(gameObject);
    }

    private void DisplaceLogs()
    {
        foreach (Transform child in gameObject.transform)
        {
            if(child.name == "Log")
            {
                child.transform.SetParent(transform.parent);
            }
        }
    }
}
