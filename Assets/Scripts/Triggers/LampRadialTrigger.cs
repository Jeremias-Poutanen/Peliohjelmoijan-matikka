using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LampRadialTrigger : MonoBehaviour
{
    [Range(0.1f, 30f)]
    public float radius = 5f;
    public GameObject targetObject;
    public bool drawGizmos = true;
    public GameObject lightObj;

    private void OnDrawGizmos()
    {
        Vector3 vecTriggerPos = transform.position;
        Vector3 vecTargetPos = targetObject.transform.position;
        Vector3 v = vecTargetPos - vecTriggerPos;

        if (drawGizmos)
        {
            MyDraw.DrawVectorAt(vecTriggerPos, v, Color.magenta, 3.0f);

            //  Draw the disc with radius
            if (v.magnitude < radius)
            {
                Handles.color = Color.magenta;
            }
            else
            {
                Handles.color = Color.white;
            }
            
            Handles.DrawWireDisc(vecTriggerPos, new Vector3(0, 1, 0), radius, 3.0f);
        }

        if (v.magnitude < radius)
        {
            lightObj.SetActive(true);
        }
        else
        {
            lightObj.SetActive(false);
        }
    }
}
