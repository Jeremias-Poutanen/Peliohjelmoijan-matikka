using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    // Modify this in the end:
    [Range(0.1f, 20f)]
    public float radius = 5f;

    public GameObject targetObject;

    private void OnDrawGizmos()
    {
        Vector3 vecTriggerPos = transform.position;
        Vector3 vecTargetPos = targetObject.transform.position;
        Vector3 v = vecTargetPos - vecTriggerPos;

        MyDraw.DrawVectorAt(Vector3.zero, vecTriggerPos, Color.white, 3.0f);
        MyDraw.DrawVectorAt(Vector3.zero, vecTargetPos, Color.white, 3.0f);

        MyDraw.DrawVectorAt(vecTriggerPos, v, Color.magenta, 3.0f);

        //  Draw the disc with radius
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(vecTriggerPos, new Vector3(0, 1, 0), radius, 3.0f);
    }
}
