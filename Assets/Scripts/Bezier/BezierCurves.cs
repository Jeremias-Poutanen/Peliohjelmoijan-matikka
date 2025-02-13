using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierCurves : MonoBehaviour
{
    [Range(0f, 1f)]
    public float t;

    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;
    public GameObject pointD;

    private void OnDrawGizmos()
    {
        Vector3 A = pointA.transform.position;
        Vector3 B = pointB.transform.position;
        Vector3 C = pointC.transform.position;
        Vector3 D = pointD.transform.position;

        Handles.color = Color.white;
        Handles.DrawLine(A, B, 2f);
        Handles.DrawLine(B, C, 2f);
        Handles.DrawLine(C, D, 2f);

        // Interpolation, 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(X, 0.1f);
        Gizmos.DrawSphere(Y, 0.1f);
        Gizmos.DrawSphere(Z, 0.1f);

        // Interpolation, 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(P, 0.1f);
        Gizmos.DrawSphere(Q, 0.1f);

        Handles.color = Color.magenta;
        Handles.DrawLine(X, Y, 2f);
        Handles.DrawLine(Y, Z, 2f);

        // Interpolation, 3rd stage
        Vector3 O = (1 - t) * P + t * Q;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(O, 0.1f);

        Handles.color = Color.black;
        Handles.DrawLine(P, Q, 2f);

        Handles.color = Color.white;
        Handles.DrawBezier(A, D, B, C, Color.white, null, 5f);
    }
}
