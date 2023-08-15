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
    protected float attackSpeed; // 공격 쿨타임
    protected float curAttackSpeed; // 공격가능까지 남은 시간

    protected int attackNumPoint;
    protected int attackNumWeight;
    protected int attackNum; // 최대 공격횟수
    protected int curAttackNum;// 남은 공격횟수

    protected int rangePoint;
    protected float rangeWeight;
    protected float range;

    protected List<bool> debuff = new List<bool>();

    protected string element;
    protected string tag0;
    protected string tag1;
    protected string tag2;

    protected string isAttackSuccess = "attackFail"; // 공격이 성공했는지, 실패했는지, 무기를 바꿔야하는지 알려주는 값

    protected int reinforceLevel;

    [SerializeField]
    protected GameObject hitbox;
    protected Collider monsterCollider;

    protected void Awake()
    {
     
        Instantiate(hitbox, gameObject.transform);
        hitbox.transform.position = gameObject.transform.position;
        hitbox.SetActive(false);

        attackNumPoint = 3;
        attackNumWeight = 1;
        attackSpeedPoint = 1;
        attackSpeedWeight = 1.5f;


        // 이 아래부터 이 함수의 끝까지는 일시적 코드임. 무기의 종류를 더 늘릴때 없앨 것

    }

    protected void OnEnable() // 이 무기가 활성화될때마다 curAttackSpeed를 0으로 초기화 => 바로 공격
    {
        curAttackSpeed = 0;
        curAttackNum = attackNum;
        Debug.Log("enable" + attackNum);
    }
  
    protected void Update()
    {
        curAttackSpeed -= Time.deltaTime;
        statUpdate();
    }

    public string attack(Animator animator) 
    {
        if(curAttackSpeed <= 0)
        {
            if(curAttackNum > 0)
            {
                animator.SetTrigger("isAttack");
                callHItbox();
                if(monsterCollider != null) 
                {
                    //이줄에 필요한 코드 : 데미지 계산해서 적에게 줌
                    //이줄에 필요한 코드 : 무기에 있는 디버프 부여
                    isAttackSuccess = "attackSuccess";
                }
                curAttackNum--;
                curAttackSpeed = attackSpeed;

           
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

    protected void statUpdate()
    {
        attackPower = attackPowerPoint * attackPowerWeight;
        attackNum = attackNumPoint * attackNumWeight;
        attackSpeed = attackSpeedPoint * attackSpeedWeight;
        range = rangePoint * rangeWeight;
    }

    protected void callHItbox()
    {
        hitbox.SetActive(true);
        monsterCollider = hitbox.GetComponent<HitboxManager>().getMonsterCollider();
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
