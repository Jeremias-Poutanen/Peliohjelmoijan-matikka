using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierRoad : MonoBehaviour
{
    public GameObject[] points;

    public Mesh2D CrossSection;

    // From -0.1 to 1.1 for demonstration purpouses
    [Range(-0.1f, 1.1f)]
    public float t = 0f;
    public bool loop = false;

    private int GetCurrentSegment()
    {
        float seg = t * GetSegments();

        return Mathf.FloorToInt(seg);
    }

    private int GetSegments()
    {
        if (loop) return points.Length;
        else return points.Length - 1;
    }

    private float AdjustTValue(int segment)
    {
        return (t - ((float)segment / (float)GetSegments())) / (1.0f / (float)GetSegments());
    }

    private Vector3 GetBezierPoint(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
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

    private Vector3 GetBezierForwardVector(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        // Interpolation, 1st stage
        Vector3 X = (1 - t) * A + t * B;
        Vector3 Y = (1 - t) * B + t * C;
        Vector3 Z = (1 - t) * C + t * D;

        // Interpolation, 2nd stage
        Vector3 P = (1 - t) * X + t * Y;
        Vector3 Q = (1 - t) * Y + t * Z;

        return (Q - P).normalized;
    }

    private void OnDrawGizmos()
    {
        if (loop)
        {
            if (t > 1f) t -= 1;
            else if (t < 0) t += 1;
        }
        else
        {
            if (t > 1f) t = 1;
            else if (t < 0) t = 0;
        }

        int segments = GetSegments();

        // Draw the bezier segments
        for (int i = 0; i < segments; i++)
        {
            if (loop && i + 1 == segments)
            {
                Vector3 a = points[i].GetComponent<BezierPoint>().GetAnchor();
                Vector3 b = points[i].GetComponent<BezierPoint>().GetControl2();
                Vector3 c = points[0].GetComponent<BezierPoint>().GetControl1();
                Vector3 d = points[0].GetComponent<BezierPoint>().GetAnchor();

                Handles.DrawBezier(a, d, b, c, Color.white, null, 5f);

                break;
            }

            Vector3 A = points[i].GetComponent<BezierPoint>().GetAnchor();
            Vector3 B = points[i].GetComponent<BezierPoint>().GetControl2();
            Vector3 C = points[i + 1].GetComponent<BezierPoint>().GetControl1();
            Vector3 D = points[i + 1].GetComponent<BezierPoint>().GetAnchor();

            Handles.DrawBezier(A, D, B, C, Color.white, null, 5f);
        }

        int segment = GetCurrentSegment();
        if (segment == segments) segment--; // t = 1.0 --> segment should be segment-1

        float myTValue = AdjustTValue(segment);

        // Get the points for THIS SEGMENT!
        Vector3 pointA;
        Vector3 pointB;
        Vector3 pointC;
        Vector3 pointD;

        if (loop && segment + 1 == segments)
        {
            pointA = points[segment].GetComponent<BezierPoint>().GetAnchor();
            pointB = points[segment].GetComponent<BezierPoint>().GetControl2();
            pointC = points[0].GetComponent<BezierPoint>().GetControl1();
            pointD = points[0].GetComponent<BezierPoint>().GetAnchor();
        }
        else
        {
            pointA = points[segment].GetComponent<BezierPoint>().GetAnchor();
            pointB = points[segment].GetComponent<BezierPoint>().GetControl2();
            pointC = points[segment + 1].GetComponent<BezierPoint>().GetControl1();
            pointD = points[segment + 1].GetComponent<BezierPoint>().GetAnchor();
        }

        Vector3 bezPoint = GetBezierPoint(pointA, pointB, pointC, pointD, myTValue);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bezPoint, 0.1f);

        // Get the forward vector
        Vector3 forward = GetBezierForwardVector(pointA, pointB, pointC, pointD, myTValue);
        MyDraw.DrawVectorAt(bezPoint, 3f * forward, Color.blue, 2f);

        // Use Vector3.up to get the "right" vector
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        MyDraw.DrawVectorAt(bezPoint, 3f * right, Color.red, 2f);

        // Use the forward vector and "right" vector to get the correcnt "up"-vector
        Vector3 up = Vector3.Cross(forward, right);
        MyDraw.DrawVectorAt(bezPoint, 3f * up, Color.green, 2f);

        // Draw the points using the cross section fo the road
        for (int i = 0; i < CrossSection.vertices.Length; i++)
        {
            // 2D-point xcoord times tight-vector + y-coord times up-vector
            Vector3 point = CrossSection.vertices[i].point.x * right + CrossSection.vertices[i].point.y * up;

            // Add the bezier point to the above
            point += bezPoint;

            // Draw the point
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
