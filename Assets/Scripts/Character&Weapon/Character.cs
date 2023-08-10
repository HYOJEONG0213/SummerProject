using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ 

  
    private CharacterMoveset characterMoveset;
 
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

 



    public void getConsumable(Consumable consumable)
    {
        if(consumables.Count < 3)
        {
            consumables.Add(consumable);
        }
        else
        {
            Debug.Log("Consumables is full!!"); // 나중에 소모품이 다 찼다는 메시지를 띄우는 걸로 바꾸기
        }
    }
    public void useConsumable()
    {
        consumables[usingConsumable].effect();
        consumables.RemoveAt(usingConsumable);

    }

    public void getWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }
    // 무기 바꾸는 함수. flag번째 무기를 weapon과 바꾼다.
    public void changeWeapon(int flag, Weapon weapon)
    {

    }

    public void interaction(Collider other)
    {
        //Debug.Log("collision");
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (other.gameObject.tag)
            {
                case "consumable":
                    getConsumable(other.gameObject.GetComponent<Consumable>());
                   // Debug.Log(other.gameObject.name);
                    Destroy(other.gameObject);

                    break;
                case "weapon":
                    getWeapon(other.gameObject.GetComponent<Weapon>());
                    Destroy(other.gameObject);

                    break;
            }

        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // 초기화
        characterMoveset = GetComponent<CharacterMoveset>();
              

    }

    // Update is called once per frame
    void Update()
    {
        characterMoveset.jumpWithGravity();
        characterMoveset.move();
       
       
    }

    private void FixedUpdate()
    {
       

    }

    private void OnTriggerStay(Collider other)
    {
        interaction(other);
    }










}
