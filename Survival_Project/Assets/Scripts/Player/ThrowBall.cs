using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform shootingPoint;
    public int resolution = 30;
    public float maxDistance = 10f;
    public float arcHeight = 1f;

    private float currentDistance = 0f; // Khoảng cách hiện tại của đường bay
    private Vector3 endPoint; // Điểm kết thúc của đường bay
    public bool isThrowing;

    void Start(){
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Tăng dần khoảng cách bay
            currentDistance = Mathf.Min(currentDistance + Time.deltaTime * maxDistance, maxDistance);
            ShowTrajectory();
            isThrowing = true;
        }
        else
        {
            lineRenderer.enabled = false;
            currentDistance = 0f; // Reset lại khoảng cách khi ngừng giữ chuột
            isThrowing = false;
        }
    }

    void ShowTrajectory()
    {
        lineRenderer.enabled = true;

        Vector3[] points = new Vector3[resolution + 1];
        Vector3 startPoint = shootingPoint.position;
        
        // Tính toán điểm kết thúc của đường bay dựa trên khoảng cách hiện tại
        endPoint = startPoint + shootingPoint.forward * currentDistance;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);
            point.y += Mathf.Sin(t * Mathf.PI) * arcHeight;
            points[i] = point;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

}
