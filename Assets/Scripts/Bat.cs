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

    IEnumerator idle()
    {
        MyAnimSetTrigger("idle");

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
                if (canAtk && IsPlayerDir())
                {
                    //�ȴ��߿� �÷��̾ ��Ÿ� �ȿ� �ְ�, ���Ͱ� �÷��̾ ���ϰ� ������ ���ݻ��·� 
                    if (Vector3.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 15f)
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
                currentState = State.idle;  //Idle ���·� ���� 
            }
        }
    }

    IEnumerator attack()
    {
        yield return null;
        canAtk = false;
        //rb.velocity = new Vector3(0, jumpPower, 0);     //Attack���°� �Ǹ� ��¦ �����ߴٰ� ���� �ִϸ��̼� ����

        MyAnimSetTrigger("attack");
        yield return Delay1000;
        currentState = State.idle;
    }

    void Fire()     //Attack �ִϸ��̼� ����߿� ���Ÿ� ������ �ϱ� ���� �Ѿ��� ����
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);    //�Ѿ� ����
        if (bulletClone != null)
        {
            Rigidbody bulletRb = bulletClone.GetComponent<Rigidbody>();
            if (bulletRb != null)   //���� �Ѿ� Ŭ���� �����ִٸ� 
            {
                bulletRb.velocity = transform.right * -transform.localScale.x * 10f;   //�Ѿ� �ӵ�
                bulletClone.transform.localScale = new Vector3(transform.localScale.x, 1f, 0); //�Ѿ� ����
            }
            bulletClone.AddComponent<BulletController>();
            //Destroy(bulletClone, 3f);   //3�ʵڿ� �ı�
        }
    }


    void FixedUpdate()
    {
        Vector3 frontVec = transform.position + transform.right * moveSpeed * 1.5f;
        if (!isHit)
        {
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //�÷��̾�� �ȸ¾Ҵٸ� �ٶ󺸴� �������� ��� �����δ�.
            bool isWallCheck0Colliding = Physics.OverlapSphere(WallCheck[0].position, 0.01f, layerMask).Length > 0;
            bool isWallCheck1Colliding = Physics.OverlapSphere(WallCheck[1].position, 0.01f, layerMask).Length > 0;


            if (isWallCheck0Colliding && !isWallCheck1Colliding &&      //0��(�Ʒ��� ��)�� TRUE, 1��(���� ��)�� FALSE��� ����!
            !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 2f, layerMask))
            {
                //Debug.Log("����!");
                //rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }
            else if (isWallCheck0Colliding)
            {
                MonsterFlip();
            }
        }



        //���Ͱ� ���������� ���� �� ���� ��ȯ
        rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);
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
            Debug.Log("Bat: �������� �Դϴ�!");
            MonsterFlip();
        }


    }

    /*protected void OnTriggerEnter(Collider collision)   //�÷��̾�� �ε�ġ�� ���ư�����
    {
        base.OnTriggerEnter(collision);
        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            MonsterFlip();
            Debug.Log("Bat�� �÷��̾�� �ε��ƽ��ϴ�!");
        }
        if (collision.transform.CompareTag("Monster"))
        {
            Bat monsterComponent = collision.GetComponent<Bat>();
            if (monsterComponent != null)
            {
                MonsterFlip();
                Debug.Log("���ͳ��� �ε��ƽ��ϴ�!");
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
            Debug.Log("źȯ�� �÷��̾�� �ε��ƽ��ϴ�!");
            Destroy(gameObject); // ���� bulletClone�� ����
        }

        if (collision.CompareTag("Platform"))
        {
            Debug.Log("źȯ�� ���ٴڰ� �ε��ƽ��ϴ�!");
            Destroy(gameObject); // ���� bulletClone�� ����
        }
    }


}