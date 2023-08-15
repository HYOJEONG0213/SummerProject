using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8
//����cs�� ���� ������ cs ��������. �ϴ� ����

public class Monster : MonoBehaviour
{
    public int currentHp = 1;
    public float moveSpeed = 5f;    //���� �̵� �ӵ�
    public float jumpPower = 10;
    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 3f;

    public int sheild = 40;   //���� ����
    public int exp = 1;
    public int power = 1;   //���� ���ݷ�

    public bool isHit = false;  //isHit: �÷��̾�� ��Ʈ �Ǿ��°�
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;

    protected Rigidbody rb;
    protected CapsuleCollider capsuleCollider;
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;     //�÷����� ������ ���̾��ũ

    protected void Awake()  // �ʿ��� ������Ʈ ��������
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Anim = GetComponent<Animator>();

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());


    }

    IEnumerator ResetCollider()         // �ݶ��̴� ����
                                        // TakeDemage�� ���� ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off�� �Ǵµ�, �̰� �ٽ� ���ִ� ����
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
        float capsuleHeight = capsuleCollider.height * 0.5f - capsuleCollider.radius; // ĸ�� ������ ��
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


    public void TakeDamage(int dam) // ���Ͱ� �÷��̾�� ��Ʈ�� �� ��Ʈ�ڽ��� off��Ű��
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
