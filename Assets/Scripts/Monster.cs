//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8
////����cs�� ���� ������ cs ��������. �ϴ� ����

//public class Monster : MonoBehaviour
//{
//    public int level=1;
//    public int hp=1;
//    public int sheild=40;   //���� ����
//    public int exp=1;
//    public int power=1;   //���� ���ݷ�
//    public float speed=5f;   //������ �̵��ӵ�
//    public float atkCoolTime = 3f;
//    public float atkCoolTimeCalc = 3f;

//    public bool isHit = false;  //isHit: �÷��̾�� ��Ʈ �Ǿ��°�
//    public bool isGround = true;
//    public bool canAtk = true;
//    public bool MonsterDirRight;

//    protected Rigidbody2D rb;
//    protected BoxCollider2D boxCollider;
//    public GameObject hitBoxCollider;
//    public Animator Anim;
//    public LayerMask layerMask;

//    protected void Awake()  // �ʿ��� ������Ʈ ��������
//    {
//        rb = GetComponent<Rigidbody2D>();
//        boxCollider = GetComponent<BoxCollider2D>();
//        Anim = GetComponent<Animator>();

//        StartCoroutine(CalcCoolTime());
//        StartCoroutine(ResetCollider());


//    }

//    IEnumerator ResetCollider()         // ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off�� �Ǵµ�, �̰� �ٽ� ���ִ� ����
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

//    public void TakeDamage (int dam) // ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off��Ű��
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
