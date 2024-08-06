using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform originPoint; // Điểm bắt đầu của đường raycast
    public float radius = 1f; // Bán kính của đường cong
    public float arcAngle = 90f; // Góc của đường cong
    public float raycastDistance = 10f; // Khoảng cách của raycast
    public LayerMask hitLayers; // LayerMask để chỉ ra các layer mà raycast sẽ va chạm

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DrawCurvedRay();
        }
    }

    void DrawCurvedRay()
    {
        Vector3 origin = originPoint.position;

        // Tính toán số lượng và khoảng cách giữa các tia raycast
        int segments = 50; // Số lượng tia raycast
        float angleStep = arcAngle / segments; // Bước góc giữa các tia raycast

        // Vẽ các tia raycast
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep - arcAngle / 2; // Góc của tia raycast
            Vector3 direction = Quaternion.Euler(0, angle, 0) * originPoint.forward; // Hướng của tia raycast

            Vector3 rayStart = origin;
            Vector3 rayEnd = origin + direction.normalized * raycastDistance;

            // Kiểm tra va chạm
            RaycastHit hit;
            if (Physics.Raycast(rayStart, direction, out hit, raycastDistance, hitLayers))
            {
                rayEnd = hit.point;
            }

            // Vẽ raycast với Debug.DrawRay hoặc Debug.DrawLine
            Debug.DrawRay(rayStart, direction * raycastDistance, Color.red); // Vẽ tia raycast

            // Nếu có va chạm, vẽ điểm va chạm
            if (Physics.Raycast(rayStart, direction, out hit, raycastDistance, hitLayers))
            {
                Debug.DrawLine(rayStart, hit.point, Color.green);
            }
            else
            {
                Debug.DrawLine(rayStart, rayEnd, Color.blue);
            }
        }
    }
}
