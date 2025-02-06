using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    // Modify this in the end:
    [Range(-1f, 1f)]
    public float threshold = 0.9f;

    public GameObject targetObject;
    public GameObject lookAtObject;

    bool isSeen;

    private void OnDrawGizmos()
    {
        // Get variables
        Vector3 vecTriggerPos = transform.position;
        Vector3 vecTargetPos = targetObject.transform.position;

        Vector3 v = vecTargetPos - vecTriggerPos;
        Vector3 l = lookAtObject.transform.position - vecTriggerPos;

        Vector3 v_hat = v / v.magnitude;
        Vector3 l_hat = l / l.magnitude;

        float dotProduct = Vector3.Dot(v_hat, l_hat);

        // Check if target is seen by enemy
        if (dotProduct > threshold) isSeen = true;
        else isSeen = false;

        // Calculate rotation for look limits
        float angle = Mathf.Acos(threshold) * 180 / Mathf.PI;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 l_limitRight = rotation * l_hat;
        // Cannot use -rotation (because quaternions??), so need to construct quaternion again
        rotation = Quaternion.Euler(0, -angle, 0);
        Vector3 l_limitLeft = rotation * l_hat;

        // Draw vectors
        MyDraw.DrawVectorAt(vecTriggerPos, v, Color.blue, 3.0f);
        MyDraw.DrawVectorAt(vecTriggerPos, l, new Color32(0xF0, 0xA5, 0x00, 0xA0), 3.0f);

        // Draw normalized l
        MyDraw.DrawVectorAt(vecTriggerPos, l_hat, new Color32(0xF0, 0xA5, 0x00, 0xA0), 3.0f);

        // Draw limits
        MyDraw.DrawVectorAt(vecTriggerPos, l_limitRight, Color.magenta, 3.0f);
        MyDraw.DrawVectorAt(vecTriggerPos, l_limitLeft, Color.magenta, 3.0f);

        // Draw unit circle and normalized v (red if seen, green if not)
        if (isSeen)
        {
            MyDraw.DrawVectorAt(vecTriggerPos, v_hat, Color.red, 3.0f);
            Handles.color = Color.red;
            Handles.DrawWireDisc(vecTriggerPos, new Vector3(0, 1, 0), 1, 3.0f);
        }
        else
        {
            MyDraw.DrawVectorAt(vecTriggerPos, v_hat, Color.green, 3.0f);
            Handles.color = Color.green;
            Handles.DrawWireDisc(vecTriggerPos, new Vector3(0, 1, 0), 1, 3.0f);
        }
    }
}
