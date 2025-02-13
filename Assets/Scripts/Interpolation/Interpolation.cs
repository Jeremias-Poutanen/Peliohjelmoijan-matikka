using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    public GameObject objA;
    public GameObject objB;

    public GameObject myObj;

    [Range(0f, 1f)]
    public float t = 0f;

    public float interpTime = 5f;

    public EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutQuad;

    private Vector3 posA = Vector3.zero;
    private Vector3 posB = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get positions of A and B
        posA = objA.transform.position;
        posB = objB.transform.position;

        // Compute t-value
        t = Time.time / interpTime;

        // limit t to max 1
        if (t > 1f)
        {
            t = 1f;
        }

        // Easing
        // t = t * t * t;

        // EasingFunction-class
        EasingFunction.Function func = EasingFunction.GetEasingFunction(ease);
        t = func(0, 1, t);

        // Interpolate
        // Vector3 pos = (1 - t) * posA + t * posB;
        Vector3 pos = Vector3.Lerp(posA, posB, t);

        // Move the game object
        myObj.transform.position = pos;
    }

    private void DrawVector(Vector3 pos, Vector3 vec, Color c, float thickness)
    {
        Handles.color = c;
        Handles.DrawLine(pos, pos + vec, thickness);
        Handles.ConeHandleCap(0, pos + vec - 0.35f * vec.normalized, Quaternion.LookRotation(vec), 0.5f, EventType.Repaint);
    }

    private void OnDrawGizmos()
    {
        // Draw vectors from origin to A and B
        DrawVector(Vector3.zero, posA, Color.blue, 3f);
        DrawVector(Vector3.zero, posB, Color.red, 3f);

        // Draw a line between A and B
        Handles.color = Color.white;
        Handles.DrawLine(posA, posB, 3f);

        // Interpolation: X = (1-t)A + tb (X, A ,B are vectors!)
        Vector3 tmpA = (1 - t) * posA;
        Vector3 tmpB = t * posB;

        // "Part A"
        DrawVector(Vector3.zero, tmpA, Color.magenta, 3f);
        // "Part B"
        DrawVector(tmpA, tmpB, Color.magenta, 3f);

        // Vector3 posX = tmpA + tmpB;
        // myObj.transform.position = posX;
    }
}
