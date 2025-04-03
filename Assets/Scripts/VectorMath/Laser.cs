using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawLine(transform.position, transform.position + transform.right * 5f, 3f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            // We actually hit something
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);

            MyDraw.DrawVectorAt(hit.point, 3f* hit.normal, Color.green, 3f);

            // Reflection
            Vector3 reflection = transform.right - 2 * (Vector3.Dot(transform.right, hit.normal)) * hit.normal;
            Vector3 reflection2 = Vector3.Reflect(transform.right, hit.normal);

            MyDraw.DrawVectorAt(hit.point, 3f* reflection, Color.red, 3f);
            MyDraw.DrawVectorAt(hit.point, 2f * reflection2, Color.blue, 1.5f);
        }
    }
}
