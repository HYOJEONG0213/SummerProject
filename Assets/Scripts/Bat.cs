using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Monster
{
    public Transform[] WallCheck;

    public enum State
    {
        Idle,
        move,
        attack
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;
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

    IEnumerator Idle()
    {
        MyAnimSetTrigger("Idle");

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
                rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);

                bool isWallCheck0Colliding = Physics.OverlapSphere(WallCheck[0].position, 0.01f, layerMask).Length > 0;
                bool isWallCheck1Colliding = Physics.OverlapSphere(WallCheck[1].position, 0.01f, layerMask).Length > 0;

                if (isWallCheck0Colliding)
                {
                    MonsterFlip();
                }
                if (canAtk && IsPlayerDir())
                {
                    //걷는중에 플레이어가 사거리 안에 있고, 몬스터가 플레이어를 향하고 있으면 공격상태로 
                    if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 15f)
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
                currentState = State.Idle;  //Idle 상태로 변경 
            }
        }
    }

    IEnumerator attack()
    {
        yield return null;
        canAtk = false;
        rb.velocity = new Vector3(0, jumpPower, 0);     //Attack상태가 되면 살짝 점프했다가 공격 애니메이션 ㄱㄱ
        MyAnimSetTrigger("attack");

        yield return Delay1000;
        currentState = State.Idle;
    }

    void Fire()     //Attack 애니메이션 재생중에 원거리 공격을 하기 위해 총알을 생성
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);    //총알 생성
        bulletClone.GetComponent<Rigidbody>().velocity = transform.right * -transform.localScale.x * 10f;   //총알 속도
        bulletClone.transform.localScale = new Vector3(transform.localScale.x, 1f); //총알 방향
    }

}
