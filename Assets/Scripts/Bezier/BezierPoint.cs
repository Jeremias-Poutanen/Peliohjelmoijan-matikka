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
    public bool mirrorControlPoints = true;
    bool hasUpdatedTempValue = false;
    Vector3 control1TempPos = Vector3.zero;
    Vector3 control2TempPos = Vector3.zero;

    public Vector3 GetAnchor()
    {
        return transform.position;
    }

    public Vector3 GetControl1()
    {
        return control1.transform.position;
    }

    public Vector3 GetControl2()
    {
        return control2.transform.position;
    }

    private void OnDrawGizmos()
    {
        if (mirrorControlPoints)
        {
            // This because can't use Start() in Editor
            if(!hasUpdatedTempValue)
            {
                control1TempPos = control1.transform.localPosition;
                control2TempPos = control2.transform.localPosition;
                hasUpdatedTempValue = true;
            }

            if (control1.transform.localPosition != control1TempPos)
            {
                control2.transform.localPosition = -control1.transform.localPosition;
                control1TempPos = control1.transform.localPosition;
            }
            else if (control2.transform.localPosition != control2TempPos)
            {
                control1.transform.localPosition = -control2.transform.localPosition;
                control2TempPos = control2.transform.localPosition;
            }
        }

        if (drawLines)
        {
            Handles.color = Color.white;
            Handles.DrawLine(transform.position, control1.transform.position, 2f);
            Handles.DrawLine(transform.position, control2.transform.position, 2f);
        }

        if (drawPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(control1.transform.position, 1f);
            Gizmos.DrawSphere(control2.transform.position, 1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}
