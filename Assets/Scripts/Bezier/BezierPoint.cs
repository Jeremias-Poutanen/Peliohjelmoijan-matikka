using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public GameObject control1;
    public GameObject control2;

    public bool drawLines = true;
    public bool drawPoints = true;

    public Vector3 getAnchor()
    {
        return transform.position;
    }

    public Vector3 getControl1()
    {
        return control1.transform.position;
    }

    public Vector3 getControl2()
    {
        return control2.transform.position;
    }

    private void OnDrawGizmos()
    {
        if (drawLines)
        {
            Handles.color = Color.white;
            Handles.DrawLine(transform.position, control1.transform.position, 2f);
            Handles.DrawLine(transform.position, control2.transform.position, 2f);
        }

        if (drawPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(control1.transform.position, 0.1f);
            Gizmos.DrawSphere(control2.transform.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}
