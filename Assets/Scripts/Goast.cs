using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goast : Monster
{
    public Transform[] wallCheck;

    private void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        jumpPower = 15f;
    }

    private void Update()
    {
        if (!isHit)
        {
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //�÷��̾�� �ȸ¾Ҵٸ� �ٶ󺸴� �������� ��� �����δ�.

            if (!Physics.CheckCapsule(wallCheck[0].position, wallCheck[1].position, 0.01f, layerMask) &&
                !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }
            else if (Physics.CheckSphere(wallCheck[1].position, 0.01f, layerMask))     //0��, 1���� üũ�� �����ϸ� �ø��� �ϵ��� ��
            {
                MonsterFlip();
            }
        }
    }

    protected void OnTriggerEnter(Collider collision)   //�÷��̾�� �ε�ġ�� ���ư�����
    {
        base.OnTriggerEnter(collision);
        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            MonsterFlip();
        }
    }
}
