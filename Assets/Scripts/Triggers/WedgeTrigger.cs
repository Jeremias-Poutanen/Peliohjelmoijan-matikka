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

    bool isInsideAngle;
    bool isSeen;
    public bool drawTriggerArea = true;
    public bool drawLimitVectors = true;
    public bool drawTargetVector = true;
    public bool drawLookAtVector = true;

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

        Vector3 topOfLimitRight = vecTriggerPos + new Vector3(l_limitRight.x * radius, l_limitRight.y + (height / 2), l_limitRight.z * radius);
        Vector3 underLimitRight = vecTriggerPos + new Vector3(l_limitRight.x * radius, l_limitRight.y - (height / 2), l_limitRight.z * radius);

        Vector3 topOfLimitLeft = vecTriggerPos + new Vector3(l_limitLeft.x * radius, l_limitLeft.y + (height / 2), l_limitLeft.z * radius);
        Vector3 underLimitLeft = vecTriggerPos + new Vector3(l_limitLeft.x * radius, l_limitLeft.y - (height / 2), l_limitLeft.z * radius);

        // Check if target is seen by enemy
        if (dotProduct > Mathf.Cos(thresholdAngle / 2 * Mathf.Deg2Rad) && vecTargetPos.y < topOfTrigger.y && vecTargetPos.y > underTrigger.y)
        {
            isInsideAngle = true;

            if (proj_v.magnitude < radius) isSeen = true;
            else isSeen = false;
        }
        else 
        {
            isInsideAngle = false;
            isSeen = false;
        }

        // Draw vectors
        if (isSeen) 
        {
            Handles.color = Color.red;
            
            // Target vector
            if (drawTargetVector) MyDraw.DrawVectorAt(vecTriggerPos, v, Color.red, 3.0f);

            // Limit vectors
            if (drawLimitVectors)
            {
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitRight, Handles.color, 3.0f);
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitLeft, Handles.color, 3.0f);
            }
        }
        else 
        {
            Handles.color = Color.white;

            // Target vector
            if (drawTargetVector) MyDraw.DrawVectorAt(vecTriggerPos, v, Color.blue, 3.0f);

            // Limit vector
            if (drawLimitVectors)
            {
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitRight, Color.magenta, 3.0f);
                MyDraw.DrawVectorAt(vecTriggerPos, radius * l_limitLeft, Color.magenta, 3.0f);
            }
        }

        // Trigger area bounds
        if (drawTriggerArea) 
        {
            Handles.DrawLine(topOfTrigger, underTrigger, 3.0f);
            Handles.DrawLine(topOfLimitRight, underLimitRight, 3.0f);
            Handles.DrawLine(topOfLimitLeft, underLimitLeft, 3.0f);

            Handles.DrawLine(topOfTrigger, topOfLimitRight, 3.0f);
            Handles.DrawLine(topOfTrigger, topOfLimitLeft, 3.0f);

            Handles.DrawLine(underTrigger, underLimitRight, 3.0f);
            Handles.DrawLine(underTrigger, underLimitLeft, 3.0f);
            

            Handles.DrawWireArc(vecTriggerPos + new Vector3(0, height/2, 0), new Vector3(0, 1, 0), l_limitLeft, thresholdAngle, radius, 3.0f);
            Handles.DrawWireArc(vecTriggerPos - new Vector3(0, height/2, 0), new Vector3(0, 1, 0), l_limitLeft, thresholdAngle, radius, 3.0f);
        }

        // Look at vector
        if (drawLookAtVector && isInsideAngle) MyDraw.DrawVectorAt(vecTriggerPos, l, Color.red, 3.0f);
        else if (drawLookAtVector) MyDraw.DrawVectorAt(vecTriggerPos, l, new Color32(252, 100, 3, 255), 3.0f);
    }
}
