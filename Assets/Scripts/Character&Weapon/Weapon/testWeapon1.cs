using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testWeapon1 : Weapon
{
    private void Awake() 
    {
        name = "testWeapon1";
        attackNumPoint = 3;
        attackNumWeight = 1;
        attackSpeedPoint = 1;
        attackSpeedWeight = 1.5f;

        curAttackSpeed = attackSpeed;
        curAttackNum = attackNum;
    }
}
