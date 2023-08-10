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

    private List<Weapon> weapons = new List<Weapon>(); // �����ϰ� �ִ� �����. �ִ� 3��
    private int usingWeapon; // ���� ���� �ִ� ������ �ε���

    private List<Consumable> consumables = new List<Consumable>();
    private int usingConsumable; // ���� ��� �ִ� �Ҹ�ǰ�� �ε���

    private List<Characteristic> characteristics = new List<Characteristic>();
    
    private CharacterEffect characterEffect;

    private List<Weapon> inventory = new List<Weapon>(); // �������� �ʰ� �ִ� �����. �̰͵��� ���Ƽ� ������������ ����ų�, ������ �� �ִ�.

 



    public void getConsumable(Consumable consumable)
    {
        if(consumables.Count < 3)
        {
            consumables.Add(consumable);
        }
        else
        {
            Debug.Log("Consumables is full!!"); // ���߿� �Ҹ�ǰ�� �� á�ٴ� �޽����� ���� �ɷ� �ٲٱ�
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
    // ���� �ٲٴ� �Լ�. flag��° ���⸦ weapon�� �ٲ۴�.
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
        // �ʱ�ȭ
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
