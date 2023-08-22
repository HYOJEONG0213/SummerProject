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
    public Transform genPoint;  //genPoint: �Ѿ� ���� ��ġ
    public GameObject Bullet;   //�Ѿ� ������

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);

    private void Awake()    //������ ������ �����ֱ�
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

        if (Random.value > 0.5f)    //50�ۼ�Ʈ Ȯ���� Flip
        {
            MonsterFlip();
        }
        yield return Delay1000;
        currentState = State.move;   //1�ʵ� Move���·� ����
    }

    IEnumerator move()
    {
        yield return null;
        float runTime = Random.Range(2f, 3f);   //�ȴ� �ð� 2~3�ʷ� �����ϰ� ������
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
                    //�ȴ��߿� �÷��̾ ��Ÿ� �ȿ� �ְ�, ���Ͱ� �÷��̾ ���ϰ� ������ ���ݻ��·� 
                    if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 15f)
                    {
                        currentState = State.attack;
                        break;
                    }
                }
            }
            yield return null;
        }
        if (currentState != State.attack)   //�ȴµ��� ���ݻ��·� ������ �ȵǾ��ٸ�
        {
            if (Random.value > 0.5f)
            {
                MonsterFlip();  //Flip�� �ϰų� 
            }
            else
            {
                currentState = State.Idle;  //Idle ���·� ���� 
            }
        }
    }

    IEnumerator attack()
    {
        yield return null;
        canAtk = false;
        rb.velocity = new Vector3(0, jumpPower, 0);     //Attack���°� �Ǹ� ��¦ �����ߴٰ� ���� �ִϸ��̼� ����
        MyAnimSetTrigger("attack");

        yield return Delay1000;
        currentState = State.Idle;
    }

    void Fire()     //Attack �ִϸ��̼� ����߿� ���Ÿ� ������ �ϱ� ���� �Ѿ��� ����
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);    //�Ѿ� ����
        bulletClone.GetComponent<Rigidbody>().velocity = transform.right * -transform.localScale.x * 10f;   //�Ѿ� �ӵ�
        bulletClone.transform.localScale = new Vector3(transform.localScale.x, 1f); //�Ѿ� ����
    }

}
