using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goast : Monster
{
    public Transform[] WallCheck;
    private bool isAttack = false;

    private void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        power = 10;
        jumpPower = 30f;
        WallCheck = new Transform[2];
        moveDirection = true;
    }



    void Update()
    {
        WallCheck[0] = transform.Find("WallCheck0");
        WallCheck[1] = transform.Find("WallCheck1");
        if (!isHit)
        {
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //�÷��̾�� �ȸ¾Ҵٸ� �ٶ󺸴� �������� ��� �����δ�.
            Vector3 frontVec = transform.position + transform.right * moveSpeed * 1.5f;

            bool isWallCheck0Colliding = Physics.OverlapSphere(WallCheck[0].position, 0.01f, layerMask).Length > 0;
            bool isWallCheck1Colliding = Physics.OverlapSphere(WallCheck[1].position, 0.01f, layerMask).Length > 0;

            //Debug.Log(isWallCheck0Colliding);
            //Debug.Log(isWallCheck1Colliding);
            //Debug.Log(Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask));

            //���� �κ�
            if (isWallCheck0Colliding && !isWallCheck1Colliding &&      //0��(�Ʒ��� ��)�� TRUE, 1��(���� ��)�� FALSE��� ����!
                !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask))
            {
                Debug.Log("����!");
                //rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }

            else if (isWallCheck0Colliding)     //0�� ���� TRUE��� ���� ��ȯ!
            {
                Debug.Log("Goast�� �� �߰�! ���� �ٲߴϴ�!");
                MonsterFlip();
            }
        }
    }

    protected void OnTriggerEnter(Collider collision)   //�÷��̾�� �ε�ġ�� ���ݻ��·�
    {
        base.OnTriggerEnter(collision);
        if (collision.transform.CompareTag("PlayerHitBox") && isAttack == false)
        {
            GetComponent<Animator>().SetTrigger("attack");
            isAttack = true;
            Invoke("AttackAnimationEnd", 1f);
        }
    }
    public void AttackAnimationEnd()
    {
        isAttack = false;
        GetComponent<Animator>().SetTrigger("idle");
        MonsterFlip();
    }


        /*protected void OnTriggerEnter(Collider collision)   //�÷��̾�� �ε�ġ�� ���ư�����
        {
            base.OnTriggerEnter(collision);
            if (collision.transform.CompareTag("PlayerHitBox"))
            {
                MonsterFlip();
                Debug.Log("Goast�� �÷��̾�� �ε��ƽ��ϴ�!");
            }
            if (collision.transform.CompareTag("Monster"))
            {
                Goast monsterComponent = collision.GetComponent<Goast>();
                if (monsterComponent != null)
                {
                    MonsterFlip();
                    Debug.Log("���ͳ��� �ε��ƽ��ϴ�!");
                }
            }
        }*/

        void FixedUpdate()
    {
        //���Ͱ� ���������� ���� �� ���� ��ȯ
        rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);
        Vector3 frontVec;
        if (moveDirection == false)
        {
            frontVec = (transform.position + transform.right * moveSpeed * 1f);
        }
        else
        {
            frontVec = (transform.position - transform.right * moveSpeed * 1f) * 1;
        }
        Debug.DrawRay(frontVec, Vector3.down * 3, new Color(0, 1, 0));      // Ray�׷��ش�. (������ġ, ��� ����, �÷���)
        RaycastHit hit;     //hit: �浹�� ��ü�� ������ �����ϴ� ����. out�� �̿��ϸ� ������ �ȴ�
        bool isGrounded = Physics.Raycast(frontVec, Vector3.down * 3, out hit, 3, LayerMask.GetMask("Platform"));   //RaycastHit: Ray�� ���� ������Ʈ, (������ġ, ��¹���, �Ÿ�)
        //Debug.Log(!Physics.Raycast(frontVec, Vector3.down * 3, out hit, 3, LayerMask.GetMask("Platform")));

        if (!isGrounded)
        {
            Debug.Log("Goast: �������� �Դϴ�!");
            MonsterFlip();
        }


    }

    public float getAttackPower() // ������ ���ݷ��� �����ϴ� �Լ�
    {
        return power;
    }
}



    //void FixedUpdate()  // ����� ������Ʈ
    //{
    //    //Move
    //    //rb.velocity = new Vector2(moveSpeed, rb.velocity.y);  // ���Ͱ� �̵��ϴ� �ӵ�

//    //Platform / Boundary üũ
//    //������Ʈ �˻��� ���� ray�� ���..

//    Vector2 frontVec = new Vector2(rb.position.x + moveSpeed * 1.5f, rb.position.y);

//    Debug.DrawRay(frontVec, Vector3.down * 2, new Color(0, 1, 0));     // Ray�׷��ش�. (������ġ, ��� ����, �÷���)
//    RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));    //RaycastHit2D: Ray�� ���� ������Ʈ, (������ġ, ��¹���, �Ÿ�, 
//    Debug.Log(raycast.collider);
//    if (raycast.collider == null)    //�����������
//    {
//        //����׿�
//        Debug.Log("�������� ����!");
//        //moveSpeed *= -1;
//        MonsterFlip();
//        //CancelInvoke();     //���� �۵� ���� ��� Invoke�� ���ߴ� �Լ�
//        //Invoke("Think", 5);
//    }
//}


//     //�÷��̾� ���⿡ �¾��� ��
//     if (rayHit.collider != null && rayHit.collider.CompareTag("weapon"))
//         TakeDamage(30);


//     //�÷��̾ �����ϸ� ����
//     /*float distanceToPlayer = Vector2.Distance(transform.position, player.position);
//     if (distanceToPlayer < attackRange)
//     {
//         AttackPlayer();
//     }*/


// }



///* void TakeDamage(int damage)
// {
//     currenthp -= damage;
//     if (currenthp <= 0)
//     {
//         Die();
//     }
// }*/

// void Die()
// {
//     Destroy(gameObject);
// }

// void AttackPlayer()
// {
//     /*
//       PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); // PlayerHealth ��ũ��Ʈ ��������

//       if (playerHealth != null)
//       {
//           playerHealth.TakeDamage(attackDamage); // �÷��̾��� TakeDamage() �Լ� ȣ��
//       }
//      */
// }

//}

