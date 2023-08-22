using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendonCut_characteristic : Characteristic
{
    public TendonCut_characteristic()
    {
        name = "Tendon Cut";
    }
    public override void constantEffect(Weapon[] weapon, int flag)
    {
        if(weapon[0].getTag0() == "cut")
        {
            weapon[0].setDebuffTrue(0);
        }
    }
}
