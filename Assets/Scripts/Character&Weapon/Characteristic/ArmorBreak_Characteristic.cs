using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBreak_Characteristic : Characteristic
{ 
    public ArmorBreak_Characteristic()
    {
        name = "Armor Break";
    }
    public override void constantEffect(Weapon[] weapon, int flag)
    {
        if (weapon[0].getTag1() == "H")
        {
            weapon[0].setDebuffTrue(1);
        }
    }
}
