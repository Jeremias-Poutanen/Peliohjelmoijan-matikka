using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicMath : MonoBehaviour
{
    /*
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
    */

    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
        Handles.ConeHandleCap(0, pos + vec - 0.35f * vec.normalized, Quaternion.LookRotation(vec), 0.5f, EventType.Repaint);
    }

    private void DrawXYAxes(Vector3 pos, float length, float thickness)
    {
        // X-axis
        DrawVector(pos, new Vector3(length, 0, 0), Color.red, thickness);

        // Y-axis
        DrawVector(pos, new Vector3(0, length, 0), Color.green, thickness);
    }

    [Header("Screen")]
    [Range(1, 3840)]
    public int screenWidth = 1920;
    [Range(1, 2160)]
    public float screenHeight = 1080;
    
    [Header("Popup")]
    // Here could be GameObject Popup etc. for position, but currently popup is drawn at the center of the screen
    [Range(1, 100)]
    public float popupWidthPercent = 60f;
    [Range(1, 100)]
    public float popupHeightPercent = 35f;
    
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

    private void DrawPopup(Vector3 pos, float widthPercent, float heightPercent, Color c, float thickness)
    {
        Handles.color = c;
        
        float xOffset = widthPercent / 100 / 2 * screenWidth;
        float yOffset = heightPercent / 100 / 2 * screenHeight;
        // X-axis lines (width)
        Handles.DrawLine(new Vector3(pos.x - xOffset, pos.y - yOffset, 0), new Vector3(pos.x + xOffset, pos.y - yOffset, 0), 4.0f);
        Handles.DrawLine(new Vector3(pos.x - xOffset, pos.y + yOffset, 0), new Vector3(pos.x + xOffset, pos.y + yOffset, 0), 4.0f);
        // Y-axis lines (height)
        Handles.DrawLine(new Vector3(pos.x - xOffset, pos.y - yOffset, 0), new Vector3(pos.x - xOffset, pos.y + yOffset, 0), 4.0f);
        Handles.DrawLine(new Vector3(pos.x + xOffset, pos.y - yOffset, 0), new Vector3(pos.x + xOffset, pos.y + yOffset, 0), 4.0f);
        
    }

    private void OnDrawGizmos()
    {
        DrawRect(Vector3.zero, screenWidth, screenHeight, Color.black, 4.0f);
        DrawPopup(new Vector3(screenWidth/2, screenHeight/2, 0), popupWidthPercent, popupHeightPercent, Color.red, 4.0f);
    }
}
