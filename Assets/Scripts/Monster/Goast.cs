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
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //플레이어에게 안맞았다면 바라보는 방향으로 계속 움직인다.
            Vector3 frontVec = transform.position + transform.right * moveSpeed * 1.5f;

            bool isWallCheck0Colliding = Physics.OverlapSphere(WallCheck[0].position, 0.01f, layerMask).Length > 0;
            bool isWallCheck1Colliding = Physics.OverlapSphere(WallCheck[1].position, 0.01f, layerMask).Length > 0;

            //Debug.Log(isWallCheck0Colliding);
            //Debug.Log(isWallCheck1Colliding);
            //Debug.Log(Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask));

            //점프 부분
            if (isWallCheck0Colliding && !isWallCheck1Colliding &&      //0번(아래쪽 벽)은 TRUE, 1번(위쪽 벽)은 FALSE라면 점프!
                !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask))
            {
                Debug.Log("점프!");
                //rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }

            else if (isWallCheck0Colliding)     //0번 벽이 TRUE라면 방향 전환!
            {
                Debug.Log("Goast가 벽 발견! 방향 바꿉니다!");
                MonsterFlip();
            }
        }
    }

    protected void OnTriggerEnter(Collider collision)   //플레이어랑 부딪치면 공격상태로
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


        /*protected void OnTriggerEnter(Collider collision)   //플레이어랑 부딪치면 돌아가도록
        {
            base.OnTriggerEnter(collision);
            if (collision.transform.CompareTag("PlayerHitBox"))
            {
                MonsterFlip();
                Debug.Log("Goast가 플레이어와 부딪쳤습니다!");
            }
            if (collision.transform.CompareTag("Monster"))
            {
                Goast monsterComponent = collision.GetComponent<Goast>();
                if (monsterComponent != null)
                {
                    MonsterFlip();
                    Debug.Log("몬스터끼리 부딪쳤습니다!");
                }
            }
        }*/

        void FixedUpdate()
    {
        //몬스터가 낭떨어지에 있을 때 방향 전환
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
        Debug.DrawRay(frontVec, Vector3.down * 3, new Color(0, 1, 0));      // Ray그려준다. (시작위치, 쏘는 방향, 컬러값)
        RaycastHit hit;     //hit: 충돌한 물체의 정보를 저장하는 변수. out을 이용하면 저장이 된다
        bool isGrounded = Physics.Raycast(frontVec, Vector3.down * 3, out hit, 3, LayerMask.GetMask("Platform"));   //RaycastHit: Ray에 닿은 오브젝트, (시작위치, 쏘는방향, 거리)
        //Debug.Log(!Physics.Raycast(frontVec, Vector3.down * 3, out hit, 3, LayerMask.GetMask("Platform")));

        if (!isGrounded)
        {
            Debug.Log("Goast: 낭떠러지 입니다!");
            MonsterFlip();
        }


    }

    public float getAttackPower() // 몬스터의 공격력을 리턴하는 함수
    {
        return power;
    }
}



    //void FixedUpdate()  // 논리기반 업데이트
    //{
    //    //Move
    //    //rb.velocity = new Vector2(moveSpeed, rb.velocity.y);  // 몬스터가 이동하는 속도

//    //Platform / Boundary 체크
//    //오브젝트 검색을 위해 ray를 쏜다..

//    Vector2 frontVec = new Vector2(rb.position.x + moveSpeed * 1.5f, rb.position.y);

//    Debug.DrawRay(frontVec, Vector3.down * 2, new Color(0, 1, 0));     // Ray그려준다. (시작위치, 쏘는 방향, 컬러값)
//    RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));    //RaycastHit2D: Ray에 닿은 오브젝트, (시작위치, 쏘는방향, 거리, 
//    Debug.Log(raycast.collider);
//    if (raycast.collider == null)    //낭떨어지라면
//    {
//        //디버그용
//        Debug.Log("낭떨어지 주의!");
//        //moveSpeed *= -1;
//        MonsterFlip();
//        //CancelInvoke();     //현재 작동 중인 모든 Invoke를 멈추는 함수
//        //Invoke("Think", 5);
//    }
//}


//     //플레이어 무기에 맞았을 때
//     if (rayHit.collider != null && rayHit.collider.CompareTag("weapon"))
//         TakeDamage(30);


//     //플레이어를 감지하면 공격
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
//       PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); // PlayerHealth 스크립트 가져오기

//       if (playerHealth != null)
//       {
//           playerHealth.TakeDamage(attackDamage); // 플레이어의 TakeDamage() 함수 호출
//       }
//      */
// }

//}

