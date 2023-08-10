using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private int attackPowerPoint;
    private float attackPowerWeight;
    private float attackPower;

    private int attackSpeedPoint;
    private float attackSpeedWeight;
    private float attackSpeed;

    private int attackNumPoint;
    private int attackNumWeight;
    private int attackNum;

    private int rangePoint;
    private float rangeWeight;
    private float range;

    private List<bool> debuff = new List<bool>();

    private string element;
    private string tag0;
    private string tag1;
    private string tag2;

    private int reinforceLevel;

    public bool attack()
    {
        return true;
    }

    public void setStatPercent(string stat, int percent)
    {

    }

    public void setStatValue(string stat, float value)
    {

    }

    public void setDebuff()
    {

    }

    public void setElement(string element)
    {

    }

    public string getTag0()
    {
        return tag0;
    }

    public string getTag1()
    {
        return tag1;
    }
    public string getTag2()
    {
        return tag2;
    }

    public void reinforceWeapon()
    {

    }
}
