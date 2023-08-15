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
            rb.velocity = new Vector3(-transform.localScale.x * moveSpeed, rb.velocity.y, 0);  //플레이어에게 안맞았다면 바라보는 방향으로 계속 움직인다.

            if (!Physics.CheckCapsule(wallCheck[0].position, wallCheck[1].position, 0.01f, layerMask) &&
                !Physics.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
            }
            else if (Physics.CheckSphere(wallCheck[1].position, 0.01f, layerMask))     //0번, 1번벽 체크가 존재하면 플립을 하도록 함
            {
                MonsterFlip();
            }
        }
    }

    protected void OnTriggerEnter(Collider collision)   //플레이어랑 부딪치면 돌아가도록
    {
        base.OnTriggerEnter(collision);
        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            MonsterFlip();
        }
    }
}
