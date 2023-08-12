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

    private List<Weapon> weapons = new List<Weapon>(); // �����ϰ� �ִ� �����. �ִ� 3��
    private int usingWeapon; // ���� ���� �ִ� ������ �ε���

    private List<Consumable> consumables = new List<Consumable>();
    private int usingConsumable; // ���� ��� �ִ� �Ҹ�ǰ�� �ε���

    private List<Characteristic> characteristics = new List<Characteristic>();

    private CharacterEffect characterEffect;

    private List<Weapon> inventory = new List<Weapon>(); // �������� �ʰ� �ִ� �����. �̰͵��� ���Ƽ� ������������ ����ų�, ������ �� �ִ�.

    private bool isAttackSuccess; // ���� ������ ������ true �ƴϸ� false.

    private bool isTrigger; // isTrigger�� �ִ� ������Ʈ�� ĳ���Ϳ� �ݶ��̴��� �´�Ҵٸ� true �ƴϸ�  false
    private Collider activatedCollider; // ���� ĳ���Ϳ� �浹�ϰ� isTrigger�� ������ ������Ʈ�� �ݶ��̴�





    // Start is called before the first frame update
    void Awake()
    {
        // �ʱ�ȭ
        characterMoveset = GetComponent<CharacterMoveset>();
        animator = GetComponent<Animator>();
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




    public void getConsumable(Consumable consumable)
    {
        if (consumables.Count < 3)
        {
            consumables.Add(consumable);
        }
        else
        {
            Debug.Log("Consumables is full!!"); // ���ٿ� �ʿ��� �ڵ� : �Ҹ�ǰ�� �� á�ٴ� �޽����� ���� �ɷ� �ٲٱ�
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
        // ���ٿ� �ʿ��� �ڵ� : �� �ڵ带 �κ��丮�� ���⸦ �ִ� �ɷ� �ٲ�� ��
    }

    // ���� �ٲٴ� �Լ�. flag��° ���⸦ weapon�� �ٲ۴�.
    public void changeWeapon(int flag, Weapon weapon)
    {

    }

    private void weaponAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (weapons.Count != 0)
            {
                switch (weapons[usingWeapon].attack(animator))
                {
                    case "attackSuccess":
                        // ���ٿ� �ʿ��� �ڵ� : ���� ������ ������ Ư�� ����
                        Debug.Log(weapons[usingWeapon].getName());
                        break;
                    case "attackFail":

                        break;
                    case "weaponChange":
                        usingWeapon++;

                        if (usingWeapon >= 3)
                        {
                            usingWeapon = 0;
                        }
                        Debug.Log(weapons[usingWeapon].getName());
                        // ���ٿ� �ʿ��� �ڵ� : �ٲ� ���� ���� ������ �����ϴ� Ư�� ����
                        break;
                    case null:

                        break;

                }
            }



        }
    }
    private void interaction()
    {
        // ��ȣ�ۿ�Ű(e)�� ������, isTrigger�� Ȱ��ȭ�� � ������Ʈ�� ����� �� �۵�
        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            // ������Ʈ�� �±װ� consumable����, weapon������ ���� �۵��� �ٸ�
            switch (activatedCollider.gameObject.tag)
            {
                // consumable => �� ������Ʈ�� Consumable ��ũ��Ʈ�� �������� �� ������Ʈ�� ����. ������Ʈ�� ������ isTrigger�� false
                case "consumable":
                    getConsumable(activatedCollider.gameObject.GetComponent<Consumable>());
                    // Debug.Log(other.gameObject.name);
                    Destroy(activatedCollider.gameObject);
                    isTrigger = false;

                    break;
                // weapon => �� ������Ʈ�� Weapon ��ũ��Ʈ�� �������� �� ������Ʈ�� ����. ������Ʈ�� ������ isTrigger�� false
                case "weapon":
                    getWeapon(activatedCollider.gameObject.GetComponent<Weapon>());
                    Destroy(activatedCollider.gameObject);
                    isTrigger = false;

                    break;
            }
        }

    }
}
    









