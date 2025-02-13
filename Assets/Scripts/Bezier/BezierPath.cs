using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BezierPath : MonoBehaviour
{
    public GameObject[] points;

    [Range(0f, 1f)]
    public float t = 0f;

    private int GetCurrentSegment()
    {
        float seg = t * GetSegments();

        return Mathf.FloorToInt(seg);
    }

    private int GetSegments()
    {
        return points.Length - 1;
    }

    private float AdjustTValue(int segment)
    {
        return (t - ((float)segment / (float)GetSegments()))/(1.0f / (float)GetSegments());
    }

    private Vector3 getBezierPoint(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        // Interpolation, 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        // Interpolation, 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        return (1 - t) * P + t * Q;
    }

    private void OnDrawGizmos()
    {
        int segments = GetSegments();

        // Draw the bezier segments
        for (int i = 0; i < segments; i++)
        {
            Vector3 A = points[i].GetComponent<BezierPoint>().getAnchor();
            Vector3 B = points[i].GetComponent<BezierPoint>().getControl2();
            Vector3 C = points[i+1].GetComponent<BezierPoint>().getControl1();
            Vector3 D = points[i+1].GetComponent<BezierPoint>().getAnchor();

            Handles.DrawBezier(A, D, B, C, Color.white, null, 5f);
        }

        int segment = GetCurrentSegment();
        if (segment == segments) segment--; // t = 1.0 --> segment should be segment-1

        float myTValue = AdjustTValue(segment);

        // Get the points for THIS SEGMENT!
        Vector3 pointA = points[segment].GetComponent<BezierPoint>().getAnchor();
        Vector3 pointB = points[segment].GetComponent<BezierPoint>().getControl2();
        Vector3 pointC = points[segment + 1].GetComponent<BezierPoint>().getControl1();
        Vector3 pointD = points[segment + 1].GetComponent<BezierPoint>().getAnchor();

        Vector3 bezPoint = getBezierPoint(pointA, pointB, pointC, pointD, myTValue);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bezPoint, 0.1f);
    }
}
