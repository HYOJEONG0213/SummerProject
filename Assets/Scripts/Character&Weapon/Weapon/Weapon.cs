using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    protected new string name;

    protected int attackPowerPoint;
    protected float attackPowerWeight;
    protected float attackPower;

    //공격속도라고 정의하긴 했지만 사실 쿨타임 개념으로 이해하는 게 좀 더 나음
    protected int attackSpeedPoint;
    protected float attackSpeedWeight;
    protected float attackSpeed; // 공격 쿨타임 - 3초면 공격하고 나서 3초 기다리고 공격 가능
    protected float curAttackSpeed; 
    // 지금의 공격 쿨타임 
    //공격을 하고 나면 이 변수에 attackSpeed값이 할당됨. update()때마다 계속 변수의 값이 줄다가 0이 되면 공격이 가능해짐

    protected int attackNumPoint;
    protected int attackNumWeight;
    protected int attackNum; // 최대 공격횟수
    protected int curAttackNum;// 남은 공격횟수

    protected int rangePoint;
    protected float rangeWeight;
    protected float range;


    protected List<bool> debuff = new List<bool>();  // 0-slow / 1-break / 2-stop / 3-poison
    // 디버프 리스트 0부터 3까지 있고 각 항의 true나 false는 지금 무기가 그 디버프를 가지고 있는지를 나타낸다.
    // ex - 0번과 3번이 true => 공격하면 무기는 슬로우 디버프와 독 디버프를 건다. 

    protected string element; // 무기의 속성
    protected string tag0; 
    protected string tag1; 
    protected string tag2;

    protected string isAttackSuccess = "attackFail"; // 공격이 성공했는지, 실패했는지, 무기를 바꿔야하는지 알려주는 값

    protected int reinforceLevel; // 무기의 강화 레벨

    [SerializeField]
    protected GameObject hitboxPrefab; // 무기가 가지고있는 히트박스 프리팹 (원본)
    protected GameObject hitbox; // 무기가 생성해서 자식으로 두는 히트박스 (복사본)
    protected Collider monsterCollider; // 히트박스에 닿은 몬스터의 콜라이더

    protected Character character; // 무기를 장착하고 있는 캐릭터의 "Character" 스크립트

    protected void Awake()
    {
     
        hitbox = Instantiate(hitboxPrefab); // 무기의 히트박스를 만들고
        hitbox.transform.SetParent(gameObject.transform, false); // 히트박스의 부모를 무기로 설정
        hitbox.SetActive(false);// 히트박스가 안보이게 만듦
        gameObject.GetComponent<WeaponDragDrop>().enabled = false; // 드래그 드랍 스크립트를 false로 설정 - 인벤토리 UI가 켜지면 켜지도록 만들려고 했음 

        //for(int i = 0; i < 4; i++)
        //{
        //    debuff[i] = false;
        //}
        // 디버프를 초기화하기 위해 만든 반복문 - 고쳐야 함

        // 이 아래부터 이 함수의 끝까지는 일시적 코드임. Weapon의 하위 클래스로 무기 종류를 구체화할 때, 그 무기의 클래스 awake에 스탯 초기화를 넣을 것. 부모인 weapon클래스에서는 필요없다.
        attackNumPoint = 3;
        attackNumWeight = 1;
        attackSpeedPoint = 1;
        attackSpeedWeight = 1.5f;
        rangePoint = 5;
        rangeWeight = 2;
        attackPowerPoint = 1;
        attackPowerWeight = 1;
       

    }

    protected void OnEnable() 
    {

        curAttackSpeed = 0;
        // 이 무기가 활성화될때마다 curAttackSpeed를 0으로 초기화
        // => 바뀌기 전 무기의 attackSpeed만큼 기다리고 난 후 공격을 누르면 다음 무기로 교체되면서 바로 공격할 수 있게 됨
        curAttackNum = attackNum;
        isAttackSuccess = "null";
        
    }
  
    protected void Update()
    {
        curAttackSpeed -= Time.deltaTime; //계속 현재 공격 쿨타임이 줄게 만듦 => 다 줄면 공격 가능
        statUpdate();
    }

    //-------------------------------------------------------------------------------//

    public string attack(Animator animator) // 공격 , 애니메이터를 파라미터로 받아 공격 애니메이션이 나가게 만듦 , 리턴값으로 공격의 상황을 스트링으로 보냄
    {
    
        if(curAttackSpeed <= 0) // 현재 공격 쿨타임이 0이거나 그보다 아래라 공격이 가능할 때
        {
 
            if(curAttackNum > 0)// 공격횟수가 아직 남았을 때
            {
                animator.SetTrigger("isAttack"); // 공격 애니메이션 작동
               
                if(monsterCollider != null) // 닿은 몬스터가 있다면
                {
                    // 이부분은 몬스터에게 데미지를 주는 부분 - 미완
                    //Monster monster = monsterCollider.gameObject.GetComponent<Monster>();
                    //float damage = (attackPower + character.getAttackPower()) * (1 - (40 / (100 + 40))); //40부분 몬스터 방어력으로 바꾸기
                    //monster.TakeDamage((int)damage); //TakeDamage()의 파라미터를 float로 고쳐야 함 , int 캐스팅 나중에 빼기
                    
                    //이줄에 필요한 코드 : 무기에 있는 디버프 부여
                    isAttackSuccess = "attackSuccess"; // 공격 성공이라고 변수 설정
                }
                curAttackNum--; // 지금 남은 공격횟수를 1회 줄임
                curAttackSpeed = attackSpeed; // 남은 공격 쿨타임을 초기화함

           
                return isAttackSuccess; // 공격의 상황(공격 성공, 실패, 무기 교체 셋 중 하나)를 보냄
                   

            }
            else// 공격횟수가 없는 상태에서 공격을 했을 때
            {
                isAttackSuccess = "weaponChange"; 
                return isAttackSuccess; // 리턴값으로 무기 교체를 보내서 무기를 바꿈
            }
           
        }
        else // 아직 공격 쿨타임이 남았을 때
        {
           
            isAttackSuccess = "null";
            return isAttackSuccess; // 그냥 null을 리턴해 아무런 일도 없음
        }
        
    }

    protected void statUpdate() // 스탯의 업데이트
    {
        attackPower = attackPowerPoint * attackPowerWeight;
        attackNum = attackNumPoint * attackNumWeight;
        attackSpeed = attackSpeedPoint * attackSpeedWeight;
        range = rangePoint * rangeWeight;
        // 스탯들은 스탯에 부여된 point와 weight의 곱으로 구성되고 매 프레임마다 그에 맞게 스탯이 유지되도록 만듦
       
        // 지금 문제는 statUpdate가 Update()에 있어서 매 프레임마다 스탯을 설정하는 건데 
        // 이렇게 되면 아래 setStatPercent나 setStatValue를 해서 스탯을 바꾼다해도 다음 프레임이면 statUpdate에 의해 원 상태로 돌아간다.
        // 그렇기에 statUpdate를 Update()에 둬 매 프레임마다 스탯을 바꾸기 보단 특정 타이밍에 스탯이 바뀌도록 만들어야 한다.
    }

    
    public void enableHitbox() //히트박스 키는 함수  - Character 클래스에서 콜함 
    {
        hitbox.SetActive(true);
       

        monsterCollider = hitbox.GetComponent<Hitbox>().getMonsterCollider();
    }

    public void disableHitbox()//히트박스 끄는 함수  - Character 클래스에서 콜함 
    {
        hitbox.SetActive(false);

    }

    public void reinforceWeapon() // 무기 강화 함수 - 미완
    {

    }

    public void enableWeaponDragDrop() // 인벤토리 UI로 들어갈 때 호출해야하는 함수 
    {
        gameObject.GetComponent<WeaponDragDrop>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        enabled = false;

    }
    //----------------------------------------------------------------------//

    public void setStatPercent(string stat, int percent) // 특정 스탯을 퍼센트만큼 늘리거나 줄이는 함수 - 미완 
    {

    }

    public void setStatValue(string stat, float value) // 특정 스탯을 일정 값만큼 늘리거나 줄이는 함수 - 미완
    {

    }
    public void setDebuffTrue(int debuffFlag) // 디버프를 키는 함수 - 주어진 debuffFlag번째의 디버프를 킨다.
    {
        debuff[debuffFlag] = true;
    }

    public void setElement(string element) // 무기 속성을 정하는 함수 - 미완
    {

    }

    public void setCharacter(Character givenCharacter) // 캐릭터 오브젝트의 Character 스크립트를 받는 함수
    {
        character = givenCharacter;
    }
    
    //------------------------------------------------------------------//

    // 아래 함수들은 get함수로 본 스크립트의 변수를 리턴하는 함수
    public string getTag0() // tag0 = 베기, 찌르기, 투사체, 지역
    {
        return tag0;
    }

    public string getTag1() // tag1 = 헤비 , 라이트, 밸런스
    {
        return tag1;
    }
    public string getTag2() // tag2 = 무기종류(ex - 창, 검, 도끼 등등)
    {
        return tag2;
    }

    public string getName()
    {
        return name;
    }

    public int getAttackPowerPoint()
    {
        return attackPowerPoint;
    }

    public float getAttackPowerWeight()
    {
        return attackPowerWeight;
    }

    public int getAttackSpeedPoint()
    {
        return attackSpeedPoint;
    }
    public float getAttackSpeedWeight()
    {
        return attackSpeedWeight;
    }


    public int getAttackNumPoint()
    {
        return attackNumPoint;
    }
    public int getAttackNumWeight()
    {
        return attackNumWeight;
    }

    public int getRangePoint()
    {
        return rangePoint;
    }
    public float getRangeWeight()
    {
        return rangeWeight;
    }
}
