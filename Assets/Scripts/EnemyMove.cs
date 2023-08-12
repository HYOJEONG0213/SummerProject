using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=7MYUOzgZTf8&t=253s

//�¿�� �Դٰ��� �ϴ� ���� �����
// https://www.youtube.com/watch?v=24zBB474Lmg&list=PL45tEBs0ZUulihZrYScIGz89EXnwBLbGl&index=8 ���� �����ص� ������

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;    // �ൿ ��ǥ�� ������ ����

    public int level = 1;
    public int maxhp = 100;
    public int currenthp;
    public int sheild = 40;   //���� ����
    public int exp = 1;
    public float speed = 5f;   //������ �̵��ӵ�

    public float attackRange = 1.5f;    //���� ����
    public int power = 1;   //���� ���ݷ�

    public Transform player;   

    void Awake()    // �ʱ�ȭ
    {
        rigid = GetComponent <Rigidbody2D> ();
        currenthp = maxhp;
        player = GameObject.FindGameObjectWithTag("Player").transform;  //�÷��̾� ã��
        Think();

        Invoke("Think", 5);          //Invoke(�־��� �ð��� ���� ��, ������ �Լ��� �����ϴ� �Լ�)

    }

    // Update is called once per frame
    void FixedUpdate()  // ����� ������Ʈ
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);  // ���Ͱ� �̵��ϴ� �ӵ�

        //Platform / Boundary üũ
        //������Ʈ �˻��� ���� ray�� ���..

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.5f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));     // Ray�׷��ش�. (������ġ, ��� ����, �÷���)
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1);    //RaycastHit2D: Ray�� ���� ������Ʈ, (������ġ, ��¹���, �Ÿ�, 
        if (rayHit.collider != null)
        {
            if (rayHit.collider == null && rayHit.collider.CompareTag("Boundary"))    //�¾Ҵٸ�
            {
                //����׿�
                //Debug.Log("�������� ����!");
                nextMove *= -1;
                CancelInvoke();     //���� �۵� ���� ��� Invoke�� ���ߴ� �Լ�
                Invoke("Think", 5);
            }
        }


        //�÷��̾� ���⿡ �¾��� ��
        if (rayHit.collider != null && rayHit.collider.CompareTag("PlayerWeapon"))
            TakeDamage(30);


        //�÷��̾ �����ϸ� ����
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
        Invoke("Think", nextThinkTime);     //��� ����
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
          PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); // PlayerHealth ��ũ��Ʈ ��������

          if (playerHealth != null)
          {
              playerHealth.TakeDamage(attackDamage); // �÷��̾��� TakeDamage() �Լ� ȣ��
          }
         */
    }

}
