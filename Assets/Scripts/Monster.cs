using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8
//몬스터cs와 몬스터 종류별 cs 만들어야함. 일단 유기

public class Monster : MonoBehaviour
{
    public int currentHp = 1;
    public float moveSpeed = 5f;    //몬스터 이동 속도
    public float jumpPower = 10;
    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 3f;

    public int sheild = 40;   //몬스터 방어력
    public int exp = 1;
    public int power = 1;   //몬스터 공격력

    public bool isHit = false;  //isHit: 플레이어에게 히트 되었는가
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;

    protected Rigidbody rb;
    protected CapsuleCollider capsuleCollider;
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;     //플랫폼을 감지할 레이어마스크

    protected void Awake()  // 필요한 컨포넌트 가져오고
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Anim = GetComponent<Animator>();

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());


    }

    IEnumerator ResetCollider()         // 콜라이더 리셋
                                        // TakeDemage에 의해 몬스터가 플레이어에게 히트될 때 히트박스가 off가 되는데, 이걸 다시 켜주는 역할
    {
        while (true)
        {
            yield return null;
            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
                hitBoxCollider.SetActive(true);
                isHit = false;
            }
        }
    }
    IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                atkCoolTimeCalc -= Time.deltaTime;
                if (atkCoolTimeCalc <= 0)
                {
                    atkCoolTimeCalc = atkCoolTime;
                    canAtk = true;
                }
            }
        }
    }


    public bool IsPlayingAnim(string AnimName)
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            return true;
        }
        return false;
    }
    public void MyAnimSetTrigger(string AnimName)
    {
        if (!IsPlayingAnim(AnimName))
        {
            Anim.SetTrigger(AnimName);
        }
    }

    protected void MonsterFlip()
    {
        MonsterDirRight = !MonsterDirRight;

        Vector3 thisScale = transform.localScale;
        if (MonsterDirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
        rb.velocity = Vector2.zero;
    }

    protected bool IsPlayerDir()
    {
        if (transform.position.x < PlayerData.Instance.Player.transform.position.x ? MonsterDirRight : !MonsterDirRight)
        {
            return true;
        }
        return false;
    }

    protected void GroundCheck()
    {
        float capsuleHeight = capsuleCollider.height * 0.5f - capsuleCollider.radius; // 캡슐 높이의 반
        Vector3 capsuleBottomCenter = transform.position - Vector3.up * capsuleHeight;

        if (Physics.CheckCapsule(capsuleBottomCenter, capsuleBottomCenter + Vector3.up * capsuleHeight * 2, capsuleCollider.radius, layerMask))
        //if (Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.size, 0, Vector2.down, 0.05f, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }


    public void TakeDamage(int dam) // 몬스터가 플레이어에게 히트될 때 히트박스를 off시키기
    {
        currentHp -= dam;
        isHit = true;
        hitBoxCollider.SetActive(false); 
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if ( collision.transform.CompareTag ("PlayerHitBox"))
        {
        TakeDamage ( 10 );
        }
    }
}
