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
    [Range(5, 100)]
    public int crossSectionCount = 10;
    public bool loop = false;
    public bool drawBezierVectors = true;

    // Current Bezier points
    Vector3 currentPointA;
    Vector3 currentPointB;
    Vector3 currentPointC;
    Vector3 currentPointD;

    private int GetCurrentSegment(float t)
    {
        float seg = t * GetSegments();

        return Mathf.FloorToInt(seg);
    }

    private int GetSegments()
    {
        if (loop) return points.Length;
        else return points.Length - 1;
    }

    private float AdjustTValue(int segment, float t)
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

        int segment = GetCurrentSegment(t);
        if (segment == segments) segment--; // t = 1.0 --> segment should be segment-1

        float myTValue = AdjustTValue(segment, t);

        UpdateCurrentBezierPoints(segment);

        Vector3 bezPoint = GetBezierPoint(currentPointA, currentPointB, currentPointC, currentPointD, myTValue);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bezPoint, 0.1f);

        // Get the forward vector
        Vector3 forward = GetBezierForwardVector(currentPointA, currentPointB, currentPointC, currentPointD, myTValue);

        // Use Vector3.up to get the "right" vector
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        // Use the forward vector and "right" vector to get the correcnt "up"-vector
        Vector3 up = Vector3.Cross(forward, right);

        if(drawBezierVectors)
        {
            MyDraw.DrawVectorAt(bezPoint, 3f * forward, Color.blue, 2f);
            MyDraw.DrawVectorAt(bezPoint, 3f * right, Color.red, 2f);
            MyDraw.DrawVectorAt(bezPoint, 3f * up, Color.green, 2f);
        }

        DrawCrossSections();
    }

    public void DrawCrossSections()
    {
        float crossTMultiplier = 1 / (float)crossSectionCount;
        
        for(int i = 0; i < crossSectionCount; i++)
        {
            float crossTValue = i * crossTMultiplier;

            int segment = GetCurrentSegment(crossTValue);
            if (segment == GetSegments()) segment--; // t = 1.0 --> segment should be segment-1

            float adjustedCrossTValue = AdjustTValue(segment, crossTValue);

            UpdateCurrentBezierPoints(segment);

            Vector3 point = GetBezierPoint(currentPointA, currentPointB, currentPointC, currentPointD, adjustedCrossTValue);

            // Get the vectors
            Vector3 forward = GetBezierForwardVector(currentPointA, currentPointB, currentPointC, currentPointD, adjustedCrossTValue);
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            Vector3 up = Vector3.Cross(forward, right);

            // Draw the points using the cross section fo the road
            for (int j = 0; j < CrossSection.vertices.Length; j+=2)
            {
                // 2D-point xcoord times tight-vector + y-coord times up-vector
                Vector3 A = CrossSection.vertices[CrossSection.lineIndices[j]].point.x * right + CrossSection.vertices[CrossSection.lineIndices[j]].point.y * up;
                Vector3 B = CrossSection.vertices[CrossSection.lineIndices[j+1]].point.x * right + CrossSection.vertices[CrossSection.lineIndices[j+1]].point.y * up;

                // Add the bezier point to the above
                A += point;
                B += point;

                // Draw the line
                Gizmos.color = Color.white;
                Gizmos.DrawLine(A, B);
            }
        }
    }

    public void UpdateCurrentBezierPoints(int segment)
    {
        if (loop && segment + 1 == GetSegments())
        {
            currentPointA = points[segment].GetComponent<BezierPoint>().GetAnchor();
            currentPointB = points[segment].GetComponent<BezierPoint>().GetControl2();
            currentPointC = points[0].GetComponent<BezierPoint>().GetControl1();
            currentPointD = points[0].GetComponent<BezierPoint>().GetAnchor();
        }
        else
        {
            currentPointA = points[segment].GetComponent<BezierPoint>().GetAnchor();
            currentPointB = points[segment].GetComponent<BezierPoint>().GetControl2();
            currentPointC = points[segment + 1].GetComponent<BezierPoint>().GetControl1();
            currentPointD = points[segment + 1].GetComponent<BezierPoint>().GetAnchor();
        }
    }
}
