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

    protected float attackDuration;

    protected List<bool> debuff = new List<bool>();

    protected string element;
    protected string tag0;
    protected string tag1;
    protected string tag2;

    protected string isAttackSuccess = "attackFail"; // 공격이 성공했는지, 실패했는지, 무기를 바꿔야하는지 알려주는 값

    protected int reinforceLevel;

    [SerializeField]
    protected GameObject hitboxPrefab; // 무기가 가지고있는 히트박스 프리팹 (원본)
    protected GameObject hitbox; // 무기가 생성해서 자식으로 두는 히트박스 (복사본)
    protected Collider monsterCollider; // 히트박스에 닿은 몬스터의 콜라이더

    protected Character character; // 무기를 장착하고 있는 캐릭터의 "Character" 스크립트

    protected void Awake()
    {
     
        hitbox = Instantiate(hitboxPrefab);
        hitbox.transform.SetParent(gameObject.transform, false);
        hitbox.SetActive(false);
        

        // 이 아래부터 이 함수의 끝까지는 일시적 코드임. 단검같이 실제 무기를 구현할때 그 무기의 클래스 awake에 스탯 초기화를 넣을 것. 부모인 weapon클래스에서는 필요없다.
        attackNumPoint = 3;
        attackNumWeight = 1;
        attackSpeedPoint = 1;
        attackSpeedWeight = 1.5f;
        rangePoint = 5;
        rangeWeight = 2;
        attackPowerPoint = 1;
        attackPowerWeight = 1;
        attackDuration = attackSpeed / 2;

    }

    protected void OnEnable() // 이 무기가 활성화될때마다 curAttackSpeed를 0으로 초기화 => 바로 공격
    {
        curAttackSpeed = 0;
        curAttackNum = attackNum;
        isAttackSuccess = "null";
        Debug.Log("enable" + curAttackNum);
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
            Debug.Log("curAttackNum is " + curAttackNum);
            if(curAttackNum > 0)
            {
                animator.SetTrigger("isAttack");
                callHitbox();
                if(monsterCollider != null) 
                {
                    
                    //Monster monster = monsterCollider.gameObject.GetComponent<Monster>();
                    //float damage = (attackPower + character.getAttackPower()) * (1 - (40 / (100 + 40))); //40부분 몬스터 방어력으로 바꾸기
                    //monster.TakeDamage((int)damage); //TakeDamage()의 파라미터를 float로 고쳐야 함 , int 캐스팅 나중에 빼기
                    
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
           
            isAttackSuccess = "null";
            return isAttackSuccess;
        }
        
    }

    protected void statUpdate()
    {
        attackPower = attackPowerPoint * attackPowerWeight;
        attackNum = attackNumPoint * attackNumWeight;
        attackSpeed = attackSpeedPoint * attackSpeedWeight;
        range = rangePoint * rangeWeight;
    }

    protected void callHitbox()
    {
        hitbox.SetActive(true);
       

        monsterCollider = hitbox.GetComponent<Hitbox>().getMonsterCollider();
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

    public void setCharacter(Character givenCharacter)
    {
        character = givenCharacter;
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
