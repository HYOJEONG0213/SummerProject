using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMoveset : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxJumpNum; // �ִ� ���� Ƚ��
    [SerializeField] private float jumpPower; // ������
    private int curJumpNum; // ���� ���� Ƚ��
    [SerializeField] private float dashSpeed; // ��� �ӵ�
    private float dashCooltime; // ��� ��Ÿ��
    

    private void Awake()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        curJumpNum = maxJumpNum;
    }
    public void jump()
    {
        // ���� - ���� ���� Ƚ���� 0ȸ �̻��̰� �����̽��� ������ �۵�
        if (curJumpNum > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isJump", true);
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curJumpNum--;
            }
        }
    }

    public void move()
    {
        float inputX = Input.GetAxis("Horizontal");

        // ĳ������ �ӵ��� inputX�� ���ϰ� �� �ӵ��� ĳ���Ϳ��� ���� 
        Vector2 velocity = new Vector2(inputX, rb.velocity.y);
        velocity.x *= movementSpeed;
        rb.velocity = velocity;

        // ĳ������ �ӵ��� ���� ���ϸ� �ִϸ��̼� ����
        if (Mathf.Abs(rb.velocity.x) < 0.3)
        {
            animator.SetBool("isRun", false);
        }
        else
        {
            animator.SetBool("isRun", true);

        }

        // ĳ������ �ӵ��� ���� �¿� ����
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
    }

    public void dash()
    {
        
        // ���. �ӵ��� ����� ������ ���, �����ų� 0�̸� �ڷ� ���
        if(rb.velocity.x > 0)
        {
            Vector2 velocity = new Vector2(Vector2.right.x, rb.velocity.y);
            velocity.x *= dashSpeed;
            rb.velocity = velocity;

        }
        else
        {
            Vector2 velocity = new Vector2(Vector2.left.x, rb.velocity.y);
            velocity.x *= dashSpeed;
            rb.velocity = velocity;
        }

        // curDashCooltime�� ��� ��Ÿ������ �ʱ�ȭ�ϰ� ������ �ð���ŭ ��� curDashCooltime���� ����.  
        // �׷��� curDashCooltime�� 0�� �ɶ����� �ڷ�ƾ�� ���ӵǰ� ��ô� �۵����� �ʴ´�.
        //float startTime = Time.time;
        //for(float curDashCooltime = dashCooltime; curDashCooltime > 0; curDashCooltime -= Time.time - startTime)
        //{
        //    yield return null;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //floor�� ������ ����Ƚ���� �����ϰ� �ִϸ��̼� ����
        if (collision.gameObject.tag == "floor")
        {
            animator.SetBool("isJump", false);
            curJumpNum = maxJumpNum;

        }
    }

}
