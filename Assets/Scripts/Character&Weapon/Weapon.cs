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
    private float attackSpeed; // 공격 쿨타임
    private float curAttackSpeed; // 공격가능까지 남은 시간

    private int attackNumPoint;
    private int attackNumWeight;
    private int attackNum; // 최대 공격횟수
    private int curAttackNum;// 남은 공격횟수

    private int rangePoint;
    private float rangeWeight;
    private float range;

    private List<bool> debuff = new List<bool>();

    private string element;
    private string tag0;
    private string tag1;
    private string tag2;

    private string isAttackSuccess = "attackFail"; // 공격이 성공했는지, 실패했는지, 무기를 바꿔야하는지 알려주는 값

    private int reinforceLevel;

    private void OnEnable() // 이 무기가 활성화될때마다 curAttackSpeed를 0으로 초기화 => 바로 공격
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
                if(true) /* 무기의 히트박스가 적에게 닿았을 때, 임시로 true로 설정*/ 
                {
                    //데미지 계산해서 적에게 줌
                    //무기에 있는 디버프 부여
                    isAttackSuccess = "attackSuccess";
                }
                curAttackNum--;
                curAttackSpeed = attackSpeed;


                animator.SetBool("isAttack", false); // 공격 애니메이션이 얼마나 지속될지 정확히 정해야 함
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
