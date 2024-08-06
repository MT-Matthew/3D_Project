using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBox : MonoBehaviour
{
    public bool playerInRange;

    public List<string> items;

    public enum BoxType
    {
        smallBox,
        BigBox
    }

    public BoxType thisBoxType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
