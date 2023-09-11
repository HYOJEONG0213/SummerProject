using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Character : MonoBehaviour
{


    protected CharacterMoveset characterMoveset; // 오브젝트의 CharacterMoveset을 받는 변수
    protected Animator animator; //오브젝트의 애니메이터를 받는 변수

    protected string characterName;

    public float healthPoint = 100;
    protected float defensivePower;
    protected float attackPower;
    //public MonsterData monsterData;     //몬스터 데이터 할당하는 배열
    //private float monsterAttackPower;    //몬스터 공격력

    protected List<GameObject> usingWeapons = new List<GameObject>(); // 장착하고 있는 무기들. 최대 3개
    protected int usingWeapon; // 현재 쓰고 있는 무기의 인덱스
    protected List<GameObject> inventory = new List<GameObject>(); // 장착하지 않고 있는 무기들. 이것들은 갈아서 무기조각으로 만들거나, 장착할 수 있다.

    protected List<GameObject> consumables = new List<GameObject>(); // 가지고 있는 소모품. 최대 3개
    protected int usingConsumable; // 현재 들고 있는 소모품의 인덱스

    protected List<Characteristic> characteristics = new List<Characteristic>(); // 특성을 담아놓는 리스트. 최대 5개 - Characteristic이 아직 미완

    protected CharacterEffect characterEffect; //  각 캐릭터가 가지는 효과를 받는 변수 - 아직 CharacterEffect가 미완


    protected bool isAttackSuccess; // 서브 공격이 맞으면 true 아니면 false.

    protected bool isTrigger; // isTrigger가 있는 오브젝트(무기, 소모품)이 캐릭터의 콜라이더와 맞닿았다면 true 아니면  false | 소모품 , 무기 상호작용 관련
    protected Collider activatedCollider; // 현재 캐릭터와 충돌한 오브젝트(무기, 소모품)의 콜라이더 | 소모품 , 무기 상호작용 관련

    protected Transform hand; // 무기가 장착되는 손 오브젝트의 트랜스폼 | 사용하는 무기가 있어야 할 위치를 지정해준다.



    // Start is called before the first frame update
    void Awake()
    {
        // 초기화
        animator = GetComponent<Animator>();
        characterMoveset = GetComponent<CharacterMoveset>();

        hand = gameObject.transform.Find("Skeletal/bone_1/bone_2/bone_3/bone_7/bone_8/bone_20"); // 캐릭터 손위치 입력 | 다른캐릭터 추가되면 바꿔야 할듯
        usingWeapon = 0;
        isTrigger = false;

        //MonsterData MonsterData = MonsterData.Instance;
        /*MonsterData monsterData;
        monsterData = MonsterData.Instance;
        foreach (GameObject monsterObject in monsterData.Monster)
        {
            Monster monsterComponent = monsterObject.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                int monsterIndex = getMonsterIndex(monsterObject);
                monsterAttackPower = monsterComponent.getAttackPower(monsterIndex);    //몬스터 공격력 가져오기
            }
        }*/
        

    }

    // Update is called once per frame
    void Update()
    {
        characterMoveset.jumpWithGravity();
        characterMoveset.move();
        //위 두 함수는 CharacterMoveset의 함수.
        interaction(); // 상호작용 
        weaponAttack(); // 무기 공격
        weaponPosition(); // 사용하는 무기의 위치를 손(hand 트랜스폼 위치)에 두도록 하는 함수

    }

    int getMonsterIndex(GameObject monsterObject)
    {
        string monsterName = monsterObject.name;
        // "Monster" 뒤에 오는 숫자를 추출하여 인덱스로 사용 (예: "Monster1" -> 인덱스 1)
        string indexString = monsterName.Replace("Monster", "");
        int monsterIndex;
        if (int.TryParse(indexString, out monsterIndex))
        {
            return monsterIndex;
        }
        else
        {
            return -1;
        }
    }

            public bool IsPlayingAnim(string AnimName)      //애니메이션 관련 함수
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            return true;
        }
        return false;
    }
    public void MyAnimSetTrigger(string AnimName)
    {
        if (!IsPlayingAnim(AnimName))
        {
            animator.SetTrigger(AnimName);
        }
    }
    /*public void TakeDamage(float dam)     //플레이어한테서 데미지를 입었을때 hp 깎고 어떤 애니메이션을?
    {
        healthPoint -= dam;   //데미지 주고

        if (healthPoint <= 0) //죽음
        {
            Debug.Log("Player Dead");
            Destroy(gameObject);
        }
    }*/

    IEnumerator HurtState()
    {
        animator.SetBool("hurt", true);

        //healthPoint -= dam;   //데미지 주고
        Debug.Log("플레이어 체력: " + healthPoint);

        if (healthPoint <= 0) //죽음
        {
            Debug.Log("Player Dead");
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(1f);
        animator.SetBool("hurt", false);
    }
    protected void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger in");

        if (other.transform.CompareTag("MonsterHitBox") || other.transform.CompareTag("Projectile"))
        {
            Debug.Log("으앙 몬스터한테 맞음");
            StartCoroutine(HurtState());    //애니메이션 hurt 상태로
            //TakeDamage(monsterAttackPower);
        }

        isTrigger = true; 
        activatedCollider = other; // 캐릭터와 닿고 isTrigger가 있는 오브젝트를 activatedCollider에 저장
    }

    protected void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger out");

        isTrigger = false;

    }





            public void getConsumable(GameObject consumable) //소모품 먹는 함수 - interaction()에서 콜함
    {
        if (consumables.Count < 3) // 최대 3개까지 소모품을 먹을 수 있음
        {
   
            consumables.Add(consumable);//소모품 추가
            Destroy(activatedCollider.gameObject);
            //소모품을 먹었으니 삭제 
            

        }
        else
        {
            Debug.Log("Consumables is full!!"); // 최대 소모품 개수를 넘겼을 때, 더이상 못 먹는다는 디버그 로그
            // 위에 필요한 코드 : 소모품이 다 찼다는 경고문을 플레이어에게 띄우는 걸로 바꾸기
        }
    }
    public void useConsumable() // 지금 들고있는(usingConsumable번인) 소모품을 사용하는 함수 - 미완
    {
        consumables[usingConsumable].GetComponent<Consumable>().effect(); 
        // usingConsumable번 소모품 오브젝트의 Consmable 클래스를 받고 소모품의 효과를 작동시키는 함수 effect()실행
        consumables.RemoveAt(usingConsumable);
        //사용한 소모품 제거
    }

    
    public void getWeapon(GameObject weapon) // 무기 얻는 함수 - interaction()에서 콜함
    {
        
              
          inventory.Add(weapon); // 무기를 인벤토리에 추가
          weapon.GetComponent<Weapon>().setCharacter(gameObject.GetComponent<Character>()); //무기에게 Character 스크립트를 주었다. 이유는 Character 관련 변수를 가져올 수 있게 하려고
          weapon.transform.SetParent(gameObject.transform.GetChild(1), false); // 무기를 캐릭터의 하위 오브젝트인 inventory(이 스크립트의 inventory 리스트가 아님)의 자식으로 만듦
          weapon.GetComponent<BoxCollider>().enabled = false; // 충돌 콜라이더 없애서 상호작용이 안되게 만듦
          weapon.SetActive(false); // 인벤토리에 들어갔으니 안보이게 만듦
          Debug.Log(inventory[0].name);
             

    
        
    }

    // 무기 바꾸는 함수. usingWeaponObject를 인벤토리에 넣고 inventoryObject를 착용한다. usingWeaponSlot번째에 있는 무기를 빼고 그 곳에 무기를 착용한다.
    public void changeWeapon(GameObject usingWeaponObject, GameObject inventoryObject, int usingWeaponSlot)
    {
        // usingWeaponObject : usingWeapons 에 있는 무기 오브젝트, inventoryObject : inventory에 있는 무기 오브젝트 , usingWeaponSlot : usingWeaponObject의 번호(usingWeaponObject가 몇번째 무기인지 나타내는 변수)
        if (usingWeaponObject != null && inventoryObject != null) // usingWeaponObject와 inventoryObject가 둘 다 존재하고 그 둘의 위치를 바꿀 때
        {
            usingWeapons.Remove(usingWeaponObject);
            usingWeapons.Add(inventoryObject);
            // 리스트 usingWeapons에서 usingWeaponObject를 지우고 inventoryObject를 추가

            usingWeaponObject.transform.SetParent(gameObject.transform.GetChild(1), false);
            // usingWeaponObject를 캐릭터의 하위 오브젝트인 inventory(리스트 inventory아님)의 자식으로 추가

            inventory.Remove(inventoryObject);
            inventory.Add(usingWeaponObject);
            // 리스트 inventory에서 inventoryObject를 삭제하고 usingWeaponObject를 추가

            inventoryObject.transform.SetParent(gameObject.transform.GetChild(0), false);
            inventoryObject.transform.SetSiblingIndex(usingWeaponSlot);
            // inventoryObject를 캐릭터의 하위 오브젝트인 usingWeapons(리스트 usingWeapons아님)의 자식으로 추가
            // 그리고 inventoryObject를 그 자식 중 순서가 usingWeaponSlot이 되도록 바꿈 
            // ex - 0 1 2 무기가 있고 1번 무기를 인벤토리의 무기와 바꿈
            //      이때 usingWeaponSlot은 1이 되고 inventoryObject는 0 1 2 중 1에 들어감
        }
        else if (usingWeaponObject == null) // usingWeapon에 빈칸이 있고 inventory에서 무기를 꺼내와 그 빈칸에 넣을 때
        {
            inventory.Remove(inventoryObject);
            usingWeapons.Add(inventoryObject);
            // inventoryObject를 리스트 inventory에서 삭제하고 리스트 usingWeapons에 추가

            inventoryObject.transform.SetParent(gameObject.transform.GetChild(0), false);
            inventoryObject.transform.SetSiblingIndex(usingWeaponSlot);
            // inventoryObject를 캐릭터의 하위 오브젝트인 usingWeapons(리스트 usingWeapons아님)의 자식으로 추가
            // 그리고 inventoryObject를 그 자식 중 순서가 usingWeaponSlot이 되도록 바꿈 
        }
        else // inventory에 빈칸이 있고 usingWeapon에서 무기를 빼서 그 빈칸에 넣을 때
        {
            usingWeapons.Remove(usingWeaponObject);
            inventory.Add(usingWeaponObject);
            // usingWeaponObject를 리스트 usingWeapons에서 빼고 리스트 inventory에 추가

            usingWeaponObject.transform.SetParent(gameObject.transform.GetChild(1), false);
            // usingWeaponObject를 캐릭터의 하위 오브젝트인 inventory(리스트 inventory아님)의 자식으로 추가

        }



        // 이 아래 반복문은 아직 미완 => 확실한 실행을 장담 못함
        Weapon[] curWeaponScript = new Weapon[3]; // 현재 usingWeapons에 있는 무기 오브젝트들의 Weapon 스크립트를 가지는 배열(리스트여도 상관없음)
        for(int i = 0; i < 3; i++) // usingWeapons에 있는 무기 오브젝트에서 스크립트를 받아오는 반복문

        {
            curWeaponScript[i] = usingWeapons[i].GetComponent<Weapon>();
        }
        for(int i = 0; i < 5; i++) // 캐릭터가 가지고 있는 5개의 특성의 constantEffect() 콜
        {

            characteristics[i].constantEffect(curWeaponScript, usingWeapon);
        }
       
    }
   

    protected void weaponAttack() // 공격 함수
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼클릭 입력을 받았을 때
        {
            if (usingWeapons.Count != 0) // 적어도 착용하는 무기가 하나라도 있을 때  
            {
                switch (usingWeapons[usingWeapon].GetComponent<Weapon>().attack(animator)) // usingWeapon번째의 무기의 attack()을 실행. 그리고 함수의 리턴값으로 case문 실행
                {
                    case "attackSuccess": // 공격이 성공했을 때
                        // 이줄에 필요한 코드 : 공격 성공과 연관된 특성 적용
                      
                        break;
                    case "attackFail":// 공격이 실패했을 때 

                        break;
                    case "weaponChange": // 한 무기의 공격횟수를 다 사용해서 무기를 교체할 때 
                        GameObject preWeapon = gameObject.transform.GetChild(usingWeapon).gameObject; // preWeapon : 바뀌기 이전 무기
                        preWeapon.SetActive(false); 

                        usingWeapon++;      
                        if (usingWeapon == usingWeapons.Count)
                        {
                           
                            usingWeapon = 0;
                        }
                        // usingWeapon을 +1함. 만약 지금 가지고있는 무기 개수보다 더 크다면 0으로 초기화

                        GameObject curWeapon = gameObject.transform.GetChild(usingWeapon).gameObject;// curWeapon : 지금 들어야 하는 무기 - usingWeapon이 +1 되었기에 preWeapon의 다음에 있는 무기가 나온다.
                        curWeapon.SetActive(true); 

                        Weapon curWeaponScript = curWeapon.GetComponent<Weapon>();  
                        
                        //for(int i = 0; i < 5; i++) // 특성에 있는 모든 일시 특성을 콜함
                        //{
                        //    characteristics[i].temporaryEffect(curWeaponScript);
                        //}
                        
                        break;
                    case "null":

                        break;

                }
            }



        }
    }

    protected void weaponPosition() // 무기의 위치를 hand 트랜스폼의 위치로 두는 함수
    {
        if(usingWeapons.Count != 0) // 적어도 착용하는 무기가 하나라도 있을 때
        {
            Transform holdingWeapon = gameObject.transform.GetChild(0).GetChild(usingWeapon); //지금 쓰고있는 무기의 트랜스폼
            holdingWeapon.position = hand.position;
            holdingWeapon.rotation = hand.rotation;
            // 지금 쓰는 무기의 위치나 회전을 hand와 똑같이 함
        }
        
    }
    protected void interaction() // 상호작용
    {
        // 상호작용키(e)가 눌리고, isTrigger가 활성화된 어떤 오브젝트와 닿았을 때 작동
        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            // 오브젝트의 태그가 consumable인지, weapon인지에 따라 작동이 다름
            switch (activatedCollider.gameObject.tag)
            {
                case "consumable": 
                    getConsumable(activatedCollider.gameObject);
                
                    
                    isTrigger = false; //  아이템에 닿았을 때 isTrigger가 true가 된 상태이고 아이템을 먹으면 그 상태가 유지되기에 일부로 false로 만들어야함

                    break;
                case "weapon":
                    getWeapon(activatedCollider.gameObject);
                    

                    isTrigger = false; //  아이템에 닿았을 때 isTrigger가 true가 된 상태이고 아이템을 먹으면 그 상태가 유지되기에 일부로 false로 만들어야함

                    break;
            }
        }

    }

   

    public float getAttackPower() // 캐릭터의 공격력을 리턴하는 함수
    {
        return attackPower;
    }

    public float getHealthPoint()   //캐릭터의 hp를 리턴하는 함수
    {
        return healthPoint;
    }

    // 아래 두 함수는 애니메이션 Attack에서 콜된다.
    public void enableHitbox()
    {
        usingWeapons[usingWeapon].GetComponent<Weapon>().enableHitbox();
    }
    public void disableHitbox()
    {
        usingWeapons[usingWeapon].GetComponent<Weapon>().disableHitbox();
    }
}
    









