using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicMath : MonoBehaviour
{
    [Header("XY-axis")]
    [Range(0.1f, 10f)]
    public float axisLenght = 3.0f;
    [Range(0.1f, 10f)]
    public float axisThickness = 3.0f;

    public GameObject VectorTo;

    private Vector3 vec;

    // Rec params
    [Header("Rectangle")]
    public GameObject Rect;
    [Range(0.1f, 20f)]
    public float width = 10f;
    [Range(0.1f, 20f)]
    public float height = 5;

    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
        Handles.ConeHandleCap(0, pos + vec - 0.35f * vec.normalized, Quaternion.LookRotation(vec), 0.5f, EventType.Repaint);
    }

    private void DrawRect(Vector3 pos, float width, float height, Color c, float thickness)
    {
        Handles.color = c;

        // X-axis lines (width)
        Handles.DrawLine(pos, pos + new Vector3(width, 0, 0), thickness);
        Handles.DrawLine(pos + new Vector3(0, height, 0), pos + new Vector3(width, height, 0), thickness);

        // Y-axis lines (height)
        Handles.DrawLine(pos, pos + new Vector3(0, height, 0), thickness);
        Handles.DrawLine(pos + new Vector3(width, 0, 0), pos + new Vector3(width, height, 0), thickness);
    }

    private void DrawXYAxes(Vector3 pos, float length, float thickness)
    {
        // X-axis
        DrawVector(pos, new Vector3(length, 0, 0), Color.red, thickness);

        // Y-axis
        DrawVector(pos, new Vector3(0, length, 0), Color.green, thickness);
    }

    private void OnDrawGizmos()
    {
        // Line to object
        vec = VectorTo.transform.position;
        DrawXYAxes(vec, 3.0f, 2.0f);
        DrawVector(Vector3.zero, vec, Color.black, 3.0f);

        // XY-axis
        DrawXYAxes(Vector3.zero, axisLenght, axisThickness);

        // Circle
        Handles.color = Color.white;
        Handles.DrawWireDisc(Vector3.zero, Vector3.back, 5.0f);

        // Rect
        vec = Rect.transform.position;
        DrawRect(vec, width, height, Color.black, 3.0f);
        DrawXYAxes(vec, 2.0f, 2.0f);
    }
}
