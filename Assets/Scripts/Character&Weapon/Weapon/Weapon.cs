using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected new string name;

    protected int attackPowerPoint;
    protected float attackPowerWeight;
    protected float attackPower;

    protected int attackSpeedPoint;
    protected float attackSpeedWeight;
    protected float attackSpeed; // ���� ��Ÿ��
    protected float curAttackSpeed; // ���ݰ��ɱ��� ���� �ð�

    protected int attackNumPoint;
    protected int attackNumWeight;
    protected int attackNum; // �ִ� ����Ƚ��
    protected int curAttackNum;// ���� ����Ƚ��

    protected int rangePoint;
    protected float rangeWeight;
    protected float range;

    protected List<bool> debuff = new List<bool>();

    protected string element;
    protected string tag0;
    protected string tag1;
    protected string tag2;

    protected string isAttackSuccess = "attackFail"; // ������ �����ߴ���, �����ߴ���, ���⸦ �ٲ���ϴ��� �˷��ִ� ��

    protected int reinforceLevel;

    protected void OnEnable() // �� ���Ⱑ Ȱ��ȭ�ɶ����� curAttackSpeed�� 0���� �ʱ�ȭ => �ٷ� ����
    {
        curAttackSpeed = 0;
    }
  
    protected void Update()
    {
        curAttackSpeed -= Time.deltaTime;
    }

    public string attack(Animator animator) 
    {
        if(curAttackSpeed <= 0)
        {
            if(curAttackNum > 0)
            {
                animator.SetBool("isAttack", true);
                if(true) /* ���ٿ� �ʿ��� �ڵ� : ������ ��Ʈ�ڽ��� ������ ����� ��, �ӽ÷� true�� ����*/
                {
                    //���ٿ� �ʿ��� �ڵ� : ������ ����ؼ� ������ ��
                    //���ٿ� �ʿ��� �ڵ� : ���⿡ �ִ� ����� �ο�
                    isAttackSuccess = "attackSuccess";
                }
                curAttackNum--;
                curAttackSpeed = attackSpeed;

                
                animator.SetBool("isAttack", false); 
                return isAttackSuccess;
                   

            }
            else
            {
                isAttackSuccess = "weaponChange";
                return isAttackSuccess;
            }
           
        }
        else
        {
            return null;
        }
        
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

    public string getName()
    {
        return name;
    }

    public void reinforceWeapon()
    {

    }
}
