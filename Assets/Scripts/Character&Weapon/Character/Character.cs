using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{


    private CharacterMoveset characterMoveset;
    private Animator animator;

    private string characterName;

    private float healthPoint;
    private float defensivePower;
    private float attackPower;

    private List<GameObject> usingWeapons = new List<GameObject>(); // 장착하고 있는 무기들. 최대 3개
    private int usingWeapon; // 현재 쓰고 있는 무기의 인덱스
    private List<GameObject> inventory = new List<GameObject>(); // 장착하지 않고 있는 무기들. 이것들은 갈아서 무기조각으로 만들거나, 장착할 수 있다.

    private List<Consumable> consumables = new List<Consumable>();
    private int usingConsumable; // 현재 들고 있는 소모품의 인덱스

    private List<Characteristic> characteristics = new List<Characteristic>();

    private CharacterEffect characterEffect;


    private bool isAttackSuccess; // 서브 공격이 맞으면 true 아니면 false.

    private bool isTrigger; // isTrigger가 있는 오브젝트가 캐릭터와 콜라이더를 맞닿았다면 true 아니면  false | 소모품 , 무기 상호작용 관련
    private Collider activatedCollider; // 현재 캐릭터와 충돌하고 isTrigger를 가지는 오브젝트의 콜라이더 | 소모품 , 무기 상호작용 관련

    private Transform hand; // 무기가 장착되는 손 오브젝트의 트랜스폼 | 사용하는 무기가 있어야 할 위치를 지정해준다.
    
   


    // Start is called before the first frame update
    void Awake()
    {
        // 초기화
        animator = GetComponent<Animator>();
        characterMoveset = GetComponent<CharacterMoveset>();
        animator = GetComponent<Animator>();
        hand = gameObject.transform.Find("Skeletal/bone_1/bone_2/bone_3/bone_7/bone_8/bone_20"); // 캐릭터 손위치 입력 | 다른캐릭터 추가되면 바꿔야 할듯
        usingWeapon = 0;
        isTrigger = false;
        for(int i = 0; i <5; i++)
        {
            characteristics[i] = new Characteristic();  
        }

    }

    // Update is called once per frame
    void Update()
    {
        characterMoveset.jumpWithGravity();
        characterMoveset.move();
        interaction();
        weaponAttack();
        weaponPosition();

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger in");
        
        isTrigger = true;
        activatedCollider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger out");

        isTrigger = false;

    }



    // consumable => 그 오브젝트의 Consumable 스크립트를 가져오고 그 오브젝트는 삭제. 오브젝트가 없으니 isTrigger는 false
    public void getConsumable(Consumable consumable)
    {
        if (consumables.Count < 3)
        {
            consumables.Add(consumable);
            Destroy(activatedCollider.gameObject);
        }
        else
        {
            Debug.Log("Consumables is full!!"); // 이줄에 필요한 코드 : 소모품이 다 찼다는 메시지를 띄우는 걸로 바꾸기
        }
    }
    public void useConsumable() // 임시
    {
        consumables[usingConsumable].effect();
        consumables.RemoveAt(usingConsumable);

    }

    
    public void getWeapon(GameObject weapon)
    {
        inventory.Add(weapon); // 무기를 인벤토리에 추가
        weapon.GetComponent<Weapon>().setCharacter(gameObject.GetComponent<Character>()); //무기에게 Character 스크립트를 주었다. 이유는 Character 관련 변수를 가져올 수 있게 하려고
        Destroy(weapon); 
        Debug.Log(inventory[0].name); 
        
    }

    // 무기 바꾸는 함수. usingWeapons에서 usingWeaponFlag번째 무기를 inventory에 있는 inventoryFlag번째 무기와 바꾼다.
    public void changeWeapon(int usingWeaponsFlag, int inventoryFlag)
    {
        GameObject tempWeapon = usingWeapons[usingWeaponsFlag];
        usingWeapons[usingWeaponsFlag] = inventory[inventoryFlag];
        inventory[inventoryFlag] = tempWeapon;
        // inventory의 inventoryFlag번째 무기와 usingWeapons의 usingWeaponsFlag번째 무기를 바꿈

        Instantiate(usingWeapons[usingWeaponsFlag], gameObject.transform.GetChild(0)).transform.SetSiblingIndex(usingWeaponsFlag);
        // inventory에서 usingWeapons로 가져온 무기 오브젝트를 생성하고
        // 생성한 객체를 캐릭터의 하위 오브젝트인 usingWeapons의 자식으로 만들고
        // 생성한 객체의 순서가 usingWeaponsFlag가 되게 한다.

        Weapon[] curWeaponScript = new Weapon[3];
        for(int i = 0; i < 3; i++)
        {
            curWeaponScript[i] = usingWeapons[i].GetComponent<Weapon>();
        }
        for(int i = 0; i < 5; i++)
        {

            characteristics[i].constantEffect(curWeaponScript, usingWeaponsFlag);
        }
        // 추가할 것 : 상시특성 적용 코드 작성하기
    }

    private void weaponAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (usingWeapons.Count != 0) // 임시 코드 => 나중에 캐릭터는 기본적으로 맨주먹을 무기로 가지게 할 것
            {
                switch (usingWeapons[usingWeapon].GetComponent<Weapon>().attack(animator))
                {
                    case "attackSuccess":
                        // 이줄에 필요한 코드 : 공격 성공과 연관된 특성 적용
                      
                        break;
                    case "attackFail":

                        break;
                    case "weaponChange":
                        GameObject preWeapon = gameObject.transform.GetChild(usingWeapon).gameObject;
                        preWeapon.SetActive(false);

                        usingWeapon++;                      
                        if (usingWeapon == usingWeapons.Count)
                        {
                           
                            usingWeapon = 0;
                        }

                        GameObject curWeapon = gameObject.transform.GetChild(usingWeapon).gameObject;
                        curWeapon.SetActive(true);

                        Weapon curWeaponScript = curWeapon.GetComponent<Weapon>();  
                        Debug.Log("this weapon name is " + curWeapon.name);
                        for(int i = 0; i < 5; i++)
                        {
                            characteristics[i].temporaryEffect(curWeaponScript);
                        }
                        // 이줄에 필요한 코드 : 바뀐 무기 덕에 조건을 만족하는 일시 특성 적용
                        break;
                    case "null":

                        break;

                }
            }



        }
    }

    private void weaponPosition()
    {
        if(usingWeapons.Count != 0) // 임시 조건문 => 맨주먹을 기본으로 가지므로 나중엔 필요없음
        {
            Transform holdingWeapon = gameObject.transform.GetChild(usingWeapon); //지금 쓰고있는 무기의 트랜스폼
            holdingWeapon.position = hand.position;
            holdingWeapon.rotation = hand.rotation;
        }
        
    }
    private void interaction()
    {
        // 상호작용키(e)가 눌리고, isTrigger가 활성화된 어떤 오브젝트와 닿았을 때 작동
        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            // 오브젝트의 태그가 consumable인지, weapon인지에 따라 작동이 다름
            switch (activatedCollider.gameObject.tag)
            {
                case "consumable":
                    getConsumable(activatedCollider.gameObject.GetComponent<Consumable>());
                
                    
                    isTrigger = false;

                    break;
                case "weapon":
                    getWeapon(activatedCollider.gameObject);
                    

                    isTrigger = false;

                    break;
            }
        }

    }

    public float getAttackPower()
    {
        return attackPower;
    }

    public void enableHitbox()
    {
        usingWeapons[usingWeapon].GetComponent<Weapon>().enableHitbox();
    }
    public void disableHitbox()
    {
        usingWeapons[usingWeapon].GetComponent<Weapon>().disableHitbox();
    }
}
    









