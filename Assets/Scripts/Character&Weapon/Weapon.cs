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
    private float attackSpeed; // ���� ��Ÿ��
    private float curAttackSpeed; // ���ݰ��ɱ��� ���� �ð�

    private int attackNumPoint;
    private int attackNumWeight;
    private int attackNum; // �ִ� ����Ƚ��
    private int curAttackNum;// ���� ����Ƚ��

    private int rangePoint;
    private float rangeWeight;
    private float range;

    private List<bool> debuff = new List<bool>();

    private string element;
    private string tag0;
    private string tag1;
    private string tag2;

    private string isAttackSuccess = "attackFail"; // ������ �����ߴ���, �����ߴ���, ���⸦ �ٲ���ϴ��� �˷��ִ� ��

    private int reinforceLevel;

    private void OnEnable() // �� ���Ⱑ Ȱ��ȭ�ɶ����� curAttackSpeed�� 0���� �ʱ�ȭ => �ٷ� ����
    {
        curAttackSpeed = 0;
    }
  
    private void Update()
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
                if(true) /* ������ ��Ʈ�ڽ��� ������ ����� ��, �ӽ÷� true�� ����*/ 
                {
                    //������ ����ؼ� ������ ��
                    //���⿡ �ִ� ����� �ο�
                    isAttackSuccess = "attackSuccess";
                }
                curAttackNum--;
                curAttackSpeed = attackSpeed;


                animator.SetBool("isAttack", false); // ���� �ִϸ��̼��� �󸶳� ���ӵ��� ��Ȯ�� ���ؾ� ��
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

    public void reinforceWeapon()
    {

    }
}
