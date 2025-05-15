using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampGizmos : MonoBehaviour
{
    public bool drawLampGizmos = true;
    public List<LampRadialTrigger> lamps;

    void OnDrawGizmos()
    {
        if (drawLampGizmos)
        {
            foreach (LampRadialTrigger lamp in lamps)
            {
                lamp.drawGizmos = true;
            }
        }
        else
        {
            foreach (LampRadialTrigger lamp in lamps)
            {
                lamp.drawGizmos = false;
            }
        }
    }
}
