//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8
////몬스터cs와 몬스터 종류별 cs 만들어야함. 일단 유기

//public class Monster : MonoBehaviour
//{
//    public int level=1;
//    public int hp=1;
//    public int sheild=40;   //몬스터 방어력
//    public int exp=1;
//    public int power=1;   //몬스터 공격력
//    public float speed=5f;   //몬스터의 이동속도
//    public float atkCoolTime = 3f;
//    public float atkCoolTimeCalc = 3f;

//    public bool isHit = false;  //isHit: 플레이어에게 히트 되었는가
//    public bool isGround = true;
//    public bool canAtk = true;
//    public bool MonsterDirRight;

//    protected Rigidbody2D rb;
//    protected BoxCollider2D boxCollider;
//    public GameObject hitBoxCollider;
//    public Animator Anim;
//    public LayerMask layerMask;

//    protected void Awake()  // 필요한 컨포넌트 가져오고
//    {
//        rb = GetComponent<Rigidbody2D>();
//        boxCollider = GetComponent<BoxCollider2D>();
//        Anim = GetComponent<Animator>();

//        StartCoroutine(CalcCoolTime());
//        StartCoroutine(ResetCollider());


//    }

//    IEnumerator ResetCollider()         // 몬스터가 플레이어에게 히트될 때 히트박스가 off가 되는데, 이걸 다시 켜주는 역할
//    {
//        while (true)
//        {
//            yield return null;
//            if (!hitBoxCollider.activeInHierarchy)
//            {
//                yield return new WaitForSeconds(0.5f);
//                hitBoxCollider.SetActive(true);
//                isHit = false;
//            }
//        }
//    }
//    IEnumerator CalcCoolTime()
//    {

//    }

//    public void TakeDamage (int dam) // 몬스터가 플레이어에게 히트될 때 히트박스를 off시키기
//    {
//        hp -= dam;
//        isHit = true;
//        hitBoxCollider.SetActive(false);
//    }

//    protected void OnTriggerEnter2D(Collider2D collision)
//    {
        
//    }

//    void Update()
//    {
        
//    }
//}
