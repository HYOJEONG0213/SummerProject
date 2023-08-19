using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    private CharacterMoveset characterMoveset;
    private Animator animator;

    private string characterName;

    private float healthPoint;
    private float defensivePower;
    private float attackPower;

    private List<Weapon> weapons = new List<Weapon>(); // 장착하고 있는 무기들. 최대 3개
    private int usingWeapon; // 현재 쓰고 있는 무기의 인덱스

    private List<Consumable> consumables = new List<Consumable>();
    private int usingConsumable; // 현재 들고 있는 소모품의 인덱스

    private List<Characteristic> characteristics = new List<Characteristic>();

    private CharacterEffect characterEffect;

    private List<Weapon> inventory = new List<Weapon>(); // 장착하지 않고 있는 무기들. 이것들은 갈아서 무기조각으로 만들거나, 장착할 수 있다.

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
        hand = gameObject.transform.Find("Skeletal/bone_1/bone_2/bone_3/bone_7/bone_8/bone_20");
        usingWeapon = 0;
        isTrigger = false;
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

    //weapon => 그 오브젝트의 Weapon 스크립트를 가져오고 그 오브젝트는 비활성화 + 캐릭터를 부모로 함.오브젝트가 없으니 isTrigger는 false
    public void getWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        activatedCollider.gameObject.SetActive(false);
        activatedCollider.gameObject.transform.SetParent(gameObject.transform, false); // 임시코드 | 부모를 나중엔 인벤토리 오브젝트로 설정하는게 좋을 듯
        
        activatedCollider.gameObject.GetComponent<BoxCollider>().enabled = false;
        activatedCollider.gameObject.transform.SetSiblingIndex(0); //임시 코드
        activatedCollider.gameObject.SetActive(true); //임시 코드
        // 이 함수의 내용을 인벤토리에 무기를 넣는 걸로 바꿔야 함
    }

    // 무기 바꾸는 함수. flag번째 무기를 weapon과 바꾼다.
    public void changeWeapon(int flag, Weapon weapon)
    {

    }

    private void weaponAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (weapons.Count != 0) // 임시 코드 => 나중에 캐릭터는 기본적으로 맨주먹을 무기로 가지게 할 것
            {
                switch (weapons[usingWeapon].attack(animator))
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
                        if (usingWeapon == weapons.Count)
                        {
                           
                            usingWeapon = 0;
                        }

                        GameObject curWeapon = gameObject.transform.GetChild(usingWeapon).gameObject;
                        curWeapon.SetActive(true);

                        Debug.Log("this weapon name is " + curWeapon.name);
                        // 이줄에 필요한 코드 : 바뀐 무기 덕에 조건을 만족하는 특성 적용
                        break;
                    case "null":

                        break;

                }
            }



        }
    }

    private void weaponPosition()
    {
        if(weapons.Count != 0) // 임시 조건문 => 맨주먹을 기본으로 가지므로 나중엔 필요없음
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
                    getWeapon(activatedCollider.gameObject.GetComponent<Weapon>());
                    

                    isTrigger = false;

                    break;
            }
        }

    }
}
    









