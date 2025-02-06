using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WedgeTrigger : MonoBehaviour
{
    [Range(0.1f, 20f)]
    public float radius = 5f;

    [Range(0, 360f)]
    public float thresholdAngle = 90f;

    [Range(0.1f, 10f)]
    public float height = 3;

    public GameObject targetObject;
    public GameObject lookAtObject;

    bool isSeen;
    public bool drawCircle = true;
    public bool drawLimitVectors = true;
    public bool drawTargetVector = true;

    private void OnDrawGizmos()
    {
        // Get variables
        Vector3 vecTriggerPos = transform.position;
        Vector3 vecTargetPos = targetObject.transform.position;

        Vector3 v = vecTargetPos - vecTriggerPos;
        Vector3 l = lookAtObject.transform.position - vecTriggerPos;

        Vector3 v_hat = v / v.magnitude;
        Vector3 l_hat = l / l.magnitude;

        Vector3 y_topOfTrigger = vecTargetPos + new Vector3(0, height / 2, 0);
        Vector3 y_underTrigger = vecTargetPos - new Vector3(0, height / 2, 0);

        float dotProduct = Vector3.Dot(v_hat, l_hat);

        // Check if target is seen by enemy
        if (dotProduct > Mathf.Cos(thresholdAngle / 2 * Mathf.Deg2Rad) && v.magnitude < radius && vecTargetPos.y < y_topOfTrigger.y && vecTargetPos.y > y_underTrigger.y) isSeen = true;
        else isSeen = false;

        // Calculate rotation for look limits
        float angle = thresholdAngle / 2;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 l_limitRight = rotation * l_hat;
        // Cannot use -rotation (because quaternions??), so need to construct quaternion again
        rotation = Quaternion.Euler(0, -angle, 0);
        Vector3 l_limitLeft = rotation * l_hat;

        // Draw vectors
        if (drawTargetVector) MyDraw.DrawVectorAt(vecTriggerPos, v, Color.blue, 3.0f);
        MyDraw.DrawVectorAt(vecTriggerPos, l, new Color32(0xF0, 0xA5, 0x00, 0xA0), 3.0f);

        // Draw normalized l
        MyDraw.DrawVectorAt(vecTriggerPos, radius * l_hat, new Color32(0xF0, 0xA5, 0x00, 0xA0), 3.0f);


        // Draw arc, limits and normalized v (red if seen, green if not)
        if (isSeen)
        {
            if (drawTargetVector) MyDraw.DrawVectorAt(vecTriggerPos, radius * v_hat, Color.red, 3.0f);

            if (drawLimitVectors)
            {
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitRight, Color.red, 3.0f);
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitLeft, Color.red, 3.0f);
            }

            Handles.color = Color.red;
        }
        else
        {
            if (drawTargetVector) MyDraw.DrawVectorAt(vecTriggerPos, radius*v_hat, Color.green, 3.0f);

            if (drawLimitVectors)
            {
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitRight, Color.green, 3.0f);
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitLeft, Color.green, 3.0f);
            }

            Handles.color = Color.green;
        }

        if (drawCircle) Handles.DrawWireArc(vecTriggerPos, new Vector3(0, 1, 0), l_limitLeft, thresholdAngle, radius, 3.0f);
    }
}
