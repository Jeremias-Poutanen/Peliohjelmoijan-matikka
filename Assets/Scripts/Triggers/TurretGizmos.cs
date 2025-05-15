using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGizmos : MonoBehaviour
{
    public bool drawTurretGizmos = true;
    public List<TurretWedgeTrigger> turrets;

    void OnDrawGizmos()
    {
        if (drawTurretGizmos)
        {
            foreach (TurretWedgeTrigger turret in turrets)
            {
                turret.drawAny = true;
            }
        }
        else
        {
            foreach (TurretWedgeTrigger turret in turrets)
            {
                turret.drawAny = false;
            }
        }
    }
}