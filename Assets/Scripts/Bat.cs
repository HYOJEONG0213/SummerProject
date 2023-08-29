using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Monster
{
    public Transform[] WallCheck;

    public enum State
    {
        idle,
        move,
        attack
    };
    public State currentState = State.idle;
    public Transform genPoint;  //genPoint: 총알 생성 위치
    public GameObject Bullet;   //총알 프리팹

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);

    private void Awake()    //몬스터의 스탯을 정해주기
    {
        base.Awake();
        moveSpeed = 1f;
        jumpPower = 15f;
        currentHp = 4;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;
        WallCheck = new Transform[2];
        WallCheck[0] = transform.Find("WallCheck0");
        WallCheck[1] = transform.Find("WallCheck1");

        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator idle()
    {
        MyAnimSetTrigger("idle");

        if (Random.value > 0.5f)    //50퍼센트 확률로 Flip
        {
            MonsterFlip();
        }
        yield return Delay1000;
        currentState = State.move;   //1초뒤 Move상태로 변경
    }

    IEnumerator move()
    {
        yield return null;
        float runTime = Random.Range(2f, 3f);   //걷는 시간 2~3초로 랜덤하게 정해줌
        while (runTime >= 0f)
        {
            runTime -= Time.deltaTime;
            MyAnimSetTrigger("move");

            if (!isHit)
            {
                if (canAtk && IsPlayerDir())
                {
                    //걷는중에 플레이어가 사거리 안에 있고, 몬스터가 플레이어를 향하고 있으면 공격상태로 
                    if (Vector3.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 15f)
                    {
                        currentState = State.attack;
                        break;
                    }
                }
            }

            yield return null;
        }
        if (currentState != State.attack)   //걷는동안 공격상태로 변경이 안되었다면
        {
            if (Random.value > 0.5f)
            {
                MonsterFlip();  //Flip을 하거나 
            }
            else
            {
                currentState = State.idle;  //Idle 상태로 변경 
            }
        }
    }

    IEnumerator attack()
    {
        yield return null;
        canAtk = false;
        //rb.velocity = new Vector3(0, jumpPower, 0);     //Attack상태가 되면 살짝 점프했다가 공격 애니메이션 ㄱㄱ

        MyAnimSetTrigger("attack");
        yield return Delay1000;
        currentState = State.idle;
    }

    void Fire()     //Attack 애니메이션 재생중에 원거리 공격을 하기 위해 총알을 생성
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);    //총알 생성
        if (bulletClone != null)
        {
            Rigidbody bulletRb = bulletClone.GetComponent<Rigidbody>();
            if (bulletRb != null)   //아직 총알 클론이 남아있다면 
            {
                bulletRb.velocity = transform.right * -transform.localScale.x * 10f;   //총알 속도
                bulletClone.transform.localScale = new Vector3(transform.localScale.x, 1f, 0); //총알 방향
            }
            bulletClone.AddComponent<BulletController>();
            //Destroy(bulletClone, 3f);   //3초뒤에 파괴
        }
    }


    void FixedUpdate()
    {
        Vector3 frontVec = transform.position + transform.right * moveSpeed * 1.5f;
        if (!isHit)
        {
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //플레이어에게 안맞았다면 바라보는 방향으로 계속 움직인다.
            bool isWallCheck0Colliding = Physics.OverlapSphere(WallCheck[0].position, 0.01f, layerMask).Length > 0;
            bool isWallCheck1Colliding = Physics.OverlapSphere(WallCheck[1].position, 0.01f, layerMask).Length > 0;


            if (isWallCheck0Colliding && !isWallCheck1Colliding &&      //0번(아래쪽 벽)은 TRUE, 1번(위쪽 벽)은 FALSE라면 점프!
            !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask))
            {
                //Debug.Log("점프!");
                //rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }
            else if (isWallCheck0Colliding)
            {
                MonsterFlip();
            }
        }



        //몬스터가 낭떨어지에 있을 때 방향 전환
        rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);
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
            Debug.Log("Bat: 낭떠러지 입니다!");
            MonsterFlip();
        }


    }

    /*protected void OnTriggerEnter(Collider collision)   //플레이어랑 부딪치면 돌아가도록
    {
        base.OnTriggerEnter(collision);
        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            MonsterFlip();
            Debug.Log("Bat가 플레이어와 부딪쳤습니다!");
        }
        if (collision.transform.CompareTag("Monster"))
        {
            Bat monsterComponent = collision.GetComponent<Bat>();
            if (monsterComponent != null)
            {
                MonsterFlip();
                Debug.Log("몬스터끼리 부딪쳤습니다!");
            }
        }
    }*/


}




public class BulletController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerHitBox"))
        {
            Debug.Log("탄환이 플레이어와 부딪쳤습니다!");
            Destroy(gameObject); // 현재 bulletClone를 제거
        }

        if (collision.CompareTag("Platform"))
        {
            Debug.Log("탄환이 땅바닥과 부딪쳤습니다!");
            Destroy(gameObject); // 현재 bulletClone를 제거
        }
    }


}