using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float maxLeft = -250f;
    public float maxRight = 250f;

    public float moveSpeed = 250f;
    public float changeFrequency = 0.01f;

    public float targetPosition;
    public bool movingRight = true;

    internal void SetDifficulty(FishData fishBiting)
    {
        switch (fishBiting.fishDifficulty)
        {
            case 1:
                moveSpeed = 250;
                return;
            case 2:
                moveSpeed = 300;
                return;
            case 3:
                moveSpeed = 350;
                return;
            case 0:
                Debug.LogError("Difficulty not found for this fish");
                moveSpeed = 250;
                return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Random.Range(maxLeft, maxRight);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,
            new Vector3(targetPosition, transform.localPosition.y, transform.localPosition.z), moveSpeed * Time.deltaTime);

        
        if (Mathf.Approximately(transform.localPosition.x, targetPosition))
        {
            targetPosition = Random.Range(maxLeft, maxRight);
        }

        if (Random.value < changeFrequency)
        {
            movingRight = !movingRight;
            targetPosition = movingRight ? maxRight : maxLeft;
        }
    }
}
