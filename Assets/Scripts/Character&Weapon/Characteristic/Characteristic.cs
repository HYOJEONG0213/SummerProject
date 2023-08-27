using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Characteristic : MonoBehaviour
{
    protected new string name;
    public Characteristic()
    {
        name = "none Characteristic";
    }
    public virtual void constantEffect(Weapon[] weapon, int flag)
    {
        
    }

    public void temporaryEffect(Weapon weapon)
    {

    }

    public void buffEffect()
    {

    }

    public void getWeaponInfo(bool isAttackSuccess, string tag)
    {

    }
}
