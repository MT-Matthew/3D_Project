using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // LineRenderer chỉ cần hai điểm
    }

    void Update()
    {
        // Cập nhật vị trí của LineRenderer
        lineRenderer.SetPosition(0, object1.position);
        lineRenderer.SetPosition(1, object2.position);
    }
}
