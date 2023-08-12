using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ 

    private Rigidbody2D rb;
    private Animator animator;
    private CharacterMoveset characterMoveset;
 
    private string characterName;
    private float healthPoint;
    private float defensivePower;
 
    private float attackPower;

    private List<Weapon> weapon = new List<Weapon>();
    private int usingWeapon;
    private List<Characteristic> characteristic = new List<Characteristic>();
    private List<Consumable> consumable = new List<Consumable>();
    private CharacterEffect characterEffect;



    public void getConsumable(Consumable consumable)
    {


    }

    // 무기 바꾸는 함수. flag번째 무기를 weapon과 바꾼다.
    public void changeWeapon(int flag, Weapon weapon)
    {

    }

    // Start is called before the first frame update
    void Awake()
    {
        // 초기화

        animator = GetComponent<Animator>();
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

    private void OnTriggerEnter(Collider other)
    {
        characterMoveset.interaction(other);
    }










}
