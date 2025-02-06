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

        Vector3 proj_v = Vector3.Project(v, new Vector3(v.x, vecTriggerPos.y, v.z));
        

        Vector3 v_hat = v / v.magnitude;
        Vector3 l_hat = l / l.magnitude;
        Vector3 proj_v_hat = proj_v / proj_v.magnitude;

        Vector3 topOfTrigger = vecTriggerPos + new Vector3(0, height / 2, 0);
        Vector3 underTrigger = vecTriggerPos - new Vector3(0, height / 2, 0);

        float dotProduct = Vector3.Dot(proj_v_hat, l_hat);

        // Calculate rotation for look limits
        float angle = thresholdAngle / 2;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 l_limitRight = rotation * l_hat;
        // Cannot use -rotation (because quaternions??), so need to construct quaternion again
        rotation = Quaternion.Euler(0, -angle, 0);
        Vector3 l_limitLeft = rotation * l_hat;

        Vector3 topOfLimitRight = l_limitRight + new Vector3(0, height / 2, 0);
        Vector3 underLimitRight = l_limitRight - new Vector3(0, height / 2, 0);

        Vector3 topOfLimitLeft = l_limitLeft + new Vector3(0, height / 2, 0);
        Vector3 underLimitLeft = l_limitLeft - new Vector3(0, height / 2, 0);

        // Check if target is seen by enemy
        if (dotProduct > Mathf.Cos(thresholdAngle / 2 * Mathf.Deg2Rad) && proj_v.magnitude < radius)
        {
            if (vecTargetPos.y < topOfTrigger.y && vecTargetPos.y > underTrigger.y) isSeen = true;
            else isSeen = false;
        }
        else isSeen = false;

        MyDraw.DrawVectorAt(vecTriggerPos, proj_v, new Color32(0xF0, 0xA5, 0x00, 0xA0), 3.0f);
        MyDraw.DrawVectorAt(vecTriggerPos, proj_v_hat, Color.magenta, 3.0f);
        Handles.color = Color.white;
        Handles.DrawLine(topOfTrigger, underTrigger, 3.0f);
        Handles.DrawLine(topOfLimitRight, underLimitRight, 3.0f);
        Handles.DrawLine(topOfLimitLeft, underLimitLeft, 3.0f);

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
