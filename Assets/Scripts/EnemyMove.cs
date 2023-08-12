using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=7MYUOzgZTf8&t=253s

//좌우로 왔다갔다 하는 몬스터 만들기
// https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8 여기 참고해도 좋을듯

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;    // 행동 지표를 결정할 변수

    public int level = 1;
    public int maxhp = 100;
    public int currenthp;
    public int sheild = 40;   //몬스터 방어력
    public int exp = 1;
    public float speed = 5f;   //몬스터의 이동속도

    public float attackRange = 1.5f;    //공격 범위
    public int power = 1;   //몬스터 공격력

    public Transform player;   

    void Awake()    // 초기화
    {
        rigid = GetComponent <Rigidbody2D> ();
        currenthp = maxhp;
        player = GameObject.FindGameObjectWithTag("Player").transform;  //플레이어 찾기
        Think();

        Invoke("Think", 5);          //Invoke(주어진 시간이 지난 뒤, 지정한 함수를 실행하는 함수)

    }

    // Update is called once per frame
    void FixedUpdate()  // 논리기반 업데이트
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);  // 몬스터가 이동하는 속도

        //Platform / Boundary 체크
        //오브젝트 검색을 위해 ray를 쏜다..

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));     // Ray그려준다. (시작위치, 쏘는 방향, 컬러값)
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1);    //RaycastHit2D: Ray에 닿은 오브젝트, (시작위치, 쏘는방향, 거리, 
        if (rayHit.collider != null)
        {
            if (rayHit.collider == null && rayHit.collider.CompareTag("Boundary"))    //맞았다면
            {
                //디버그용
                //Debug.Log("낭떨어지 주의!");
                nextMove *= -1;
                CancelInvoke();     //현재 작동 중인 모든 Invoke를 멈추는 함수
                Invoke("Think", 5);
            }
        }


        //플레이어 무기에 맞았을 때
        if (rayHit.collider != null && rayHit.collider.CompareTag("PlayerWeapon"))
            TakeDamage(30);


        //플레이어를 감지하면 공격
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }

            
    }



    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2, 5);
        Invoke("Think", nextThinkTime);     //재귀 ㄱㄱ
    }

    void TakeDamage(int damage)
    {
        currenthp -= damage;
        if (currenthp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void AttackPlayer()
    {
        /*
          PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); // PlayerHealth 스크립트 가져오기

          if (playerHealth != null)
          {
              playerHealth.TakeDamage(attackDamage); // 플레이어의 TakeDamage() 함수 호출
          }
         */
    }

}
