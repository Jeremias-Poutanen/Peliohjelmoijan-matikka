using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioning : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            // We actually hit something
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);

            // Draw the normal vector
            MyDraw.DrawVectorAt(hit.point, 3f * hit.normal, Color.green, 3f);

            // Draw the look direction
            MyDraw.DrawVectorAt(hit.point, 3f * transform.right, Color.magenta, 3f);

            // Use cross-product to get "right"-vector (LEFT HAND RULE)
            Vector3 right = Vector3.Cross(hit.normal, transform.right).normalized;

            // Draw the "right"-vector
            MyDraw.DrawVectorAt(hit.point, 3f * right, Color.red, 3f);

            // Use cross-product to get "forward"-vector
            Vector3 forward = Vector3.Cross(right, hit.normal).normalized;

            // Draw the "forward"-vector
            MyDraw.DrawVectorAt(hit.point, 3f * forward, Color.blue, 3f);
        }
    }
}
